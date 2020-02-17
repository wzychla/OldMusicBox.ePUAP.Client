# OldMusicBox.ePUAP.Client

The goal of this project is to provide an independent .NET [ePUAP](https://epuap.gov.pl/wps/portal/english) Client. The implementation follows the 
[official specification](https://epuap.gov.pl/wps/portal/strefa-urzednika/pomoc_urzednik/) (*Dla integratorów* section).

## Current Version: 0.50

Please refer to the change list and the road map below.

## Features:

|  Feature  | Status |
|----|:---:|
|Single Sign On|**yes**|
|getTpUserInfo|**yes**|
|tpSigning|not yet|
|Single Log Out|not yet|
|other services|not yet|
|.NET Framework|4.6.2+|
|.NET Core|not yet|

## Documentation

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

* 1.0 
    * tpSigning - signing/verification 

* later on

    * other services
