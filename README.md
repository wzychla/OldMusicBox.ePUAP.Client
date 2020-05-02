# OldMusicBox.ePUAP.Client

The goal of this project is to provide an independent .NET [ePUAP](https://epuap.gov.pl/wps/portal/english) Client. The implementation follows the 
[official specification](https://epuap.gov.pl/wps/portal/strefa-urzednika/pomoc_urzednik/) (*Dla integratorów* section).

## Current Version: 0.58

Please refer to the change list and the road map below.

## Features:

|  Feature  | Status |
|----|:---:|
|NuGet|**yes**|
|Single Sign On|**yes**|
|getTpUserInfo|**yes**|
|tpSigning|**yes**|
|WS-pull|**partial**|
|WS-Skrytka|not yet|
|Single Log Out|not yet|
|.NET Framework|4.6.2+|
|.NET Core|not yet|

## Documentation

### Installation

The package is [available at NuGet](https://www.nuget.org/packages/OldMusicBox.ePUAP.Client). Install with the Package-Manager

```
Install-Package OldMusicBox.ePUAP.Client 
```

### ePUAP SSO

ePUAP SSO is based on SAML2 ARTIFACT binding, however it involves one extra step that complicates the implementation. 

Typical SAML2 providers rely on REDIRECT/POST binding. Both consist in the client passing the authentication request (`AuthnRequest`) to the server and server returning back the SAML2 token that contains claims (e.g. username).

The ARTIFACT binding is more complicated. Instead of just getting claims, the client gets the **artifact** (think of it as a unique, one-time token) that has to be exchanged for the token in the extra call from the client to the server (`ArtifactResolve`).

ePUAP goes a step further. Instead of returning the SAML token from the artifact call, it returns an atrofic token that contains just a single claim - the session id. The client app can't do much with this information so that it has to call yet another extra service that is beyond SAML2 scope. This service is called `getTpUserInfo` and is implemented as a WS-Security service. This service returns the information about the user of the current session.

![ePUAP SSO on sequence diagram](ePUAP_SSO.png)

Based on my understanding of how SAML2 works, this extra step is reduntant and should not be required. The artifact call, signed by the client app and issued from the server, should be enough to make sure a legit client is behind the handshake. If someone's concern was the security of the data - the ARTIFACT binding makes sure the user data never pass through
the browser but rather are requested by the application server. I don't know then what's this extra step is for, it complicates the flow without any obvious advantages.

The docs are available at ePUAP website: *Strefa urzędnika / Pomoc / Dla integratorów / Specyfikacja WSDL / Instrukcja dla integratora PZ* and *Instrukcja dla integratora DT*.
The docs are not that easy to follow, especially the WS-Security part was a trial and error loop for a couple of days.

### ePUAP in .NET

ePUAP in .NET is difficult because

* the base class library doesn't contain the SAML2 client
* the base class library doesn't support the WS-Security format ePUAP expects from the client (in theory - it should be possible with a WCF client that uses a custom binding; in practice - I was not able to find any combination of a custom binding parameters that would match the format ePUAP expects)

Any ePUAP client needs both then to succesfully implement SSO (and other services).

The [OldMusicBox.ePUAP.Client](https://github.com/wzychla/OldMusicBox.ePUAP.Client) implements the WS-Security part. SAML2 is implemented in [OldMusicBox.SAML2](https://github.com/wzychla/OldMusicBox.Saml2).

### Obtaining the certificate

You need the certificate that would be used to sign all requests that your app sends to ePUAP. The certificate can be obtained:
* by a developer themselves - only at the [test instance of ePUAP](https://int.epuap.gov.pl)
* by a local authority - at the [production instance of ePUAP](https://epuap.gov.pl)

The certificate is requested at *Strefa urzędnika* / *Udostępnianie usług* / *Wniosek o certyfikat dla środowiska integracyjnego*. The application (wniosek) requests following information:
* *Nazwa systemu teleinformatycznego* - you need to provide a name of your system
* *Adres domeny lub stały numer IP systemu, który będzie uzyskiwał dostęp do ePUAP:* - you need to provide the host header of your application
* * CSR do wystawienia certyfikatu (Instrukcja generowania żądania certyfikatu znajduje się w POMOCY na ePUAP)* - certificate request that you create locally using the `keytool.exe` from JRE, the detailed description of this is provided at ePUAP site 
* check *Uprawnienia do podpisywania i weryfikacji podpisu* and *Uprawnienia do logowania SSO*
* in the *Uprawnienia do logowania* section you need to provide:
    * *Issuer (nazwa SAML)* - this is a unique identifier of your app, could be of a form `https://your.host.name`
    * *Adresy zwrotne dla usługi SSO* - this refers to the SSO endpoint in your app where the SAML artifact is posted. Must be of a form `https://your.host.name/account/logon` (the local path depends on where the SSO endpoint is located in your app)
    * *Adres zwrotny dla usługi SLO* - this refers to the SLO (single log out) in your app where the `LogoutRequest`/`LogoutResponse` are sent. Must be of a form `https://your.host.name/account/logoff` (again, depends on where the SLO actually is in your app)

When the application is submitted it is placed in your account's *Elektroniczna Skrzynka Podawcza* where you can review it. After it's reviewed by the COI, you are emailed with the actual certificate that you put in the keystore again using the `keytool`. The result is a keystore (a `.p12` file) that contains both the certificate and the private key.

Without the certificate, there's no way to talk to ePUAP services!

### Configuring your application

Your application has to be configured - if you follow the demo provided in this repo, pay attention to configuration parameters:

* `.p12` file location
* `.p12` file password
* IssuerName
* Issuer SSO endpoint
* ePUAP SSO endpoint
* ePUAP SLO endpoint
* ePUAP artifact endpoint
* ePUAP `getTpUserInfo` endpoint
* ePUAP `tpSigning` endpoint (not required for SSO)

## Version History:

* 0.58 (2020-05-02)
    * partial support for WS-Pull and another important milestone reached. The very first succesfull response from one of ESP 
    (*Elektroniczna Skrzynka Podawcza*) services (the `oczekujaceDokumenty`). Expect more working services shortly.

* 0.57
    * `verifySignedDocument`
    * refactored services so that the service Uri is now a constructor parameter

* 0.56
    * started working on `WS-Skrytka` and `WS-pull`

* 0.55
    * tpSigning - signing/verification 

* 0.50
    - important milestone. Single Sign On already works.
    - exchaning the SAML2 token's session index for user info by calling the `getTpUserInfo`

* 0.47
    - the response parses correctly. It's just the inner XML left to parse.

* 0.46
    - corrected. I get correct response. Just to add the deserialization
    and the SSO is there.

* 0.45
	- most of `getTpUserInfo` code is there, however the server
    still returns 500 with an error message. Will correct this in the
    next commit.

* 0.4
    - reading the artifact

* 0.3
    - making the request to the Artifact endpoint

* 0.2
    - SAML2 `AuthnRequest` so that the ePUAP login endpoint picks the request and shows the login page

* 0.11
    - infrastructure elements added

* 0.1

    - initial commit 

## Roadmap

* 0.6

    - issuing `LogoutReuqest` for federated logoff

* later on

    * other services
