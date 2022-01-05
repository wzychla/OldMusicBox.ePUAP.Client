# Instrukcja generowania certyfikatu na �rodowisku testowym, skopiowana z dokumentu 

https://epuap.gov.pl/wps/wcm/connect/92c64f1f-081e-4641-8bc8-651cb8205fac/ePUAP+-+Instrukcja+generowania+%C5%BC%C4%85dania+certyfikatu+do+komunikacji+z+ePUAP+oraz+PZ_v1.1.pdf?MOD=AJPERES

Instrukcja na stronie ePUAP, po zalogowaniu, w opisie procesu **Wniosek o certyfikat do �rodowiska integracyjnego ePUAP/PZ**

```
Aby z�o�y� wniosek o certyfikat musisz by� zalogowany do podmiotu, kt�ry ma nadan� rol� Instytucja publiczna w �rodowisku int.epuap.gov.pl. 
Je�li nie masz za�o�onego takiego podmiotu a pracujesz w podmiocie publicznym lub pracujesz w firmie, kt�ra tworzy integracje dla podmiot�w 
publicznych to mo�esz utworzy� podmiot publiczny w �rodowisku int.epuap.gov.pl. Po utworzeniu podmiotu wy�lij mail 
na adres int-epuap-pomoc@coi.gov.pl z pro�b� o nadanie roli Instytucja publiczna. W mailu podaj ID podmiotu, kt�ry utworzy�e�. 
Wniosek o certyfikat b�dziesz m�g� z�o�y� gdy otrzymasz od nas informacj�, �e nadali�my Twojej organizacji rol� Instytucji publicznej.
```

* Krok 1. Utworzenie keystore - CreateKeyStore.bat, has�o Demo1234
* Krok 2. Utworzenie ��dania  - CreateCSR.bat

* Krok 3. To co przy�l� nale�y po��czy� do keystore - Import.bat

-------------------------------------------------

Na stronie generowania zaznaczam te� �e maj� by� inne us�ugi i oni pisz� tak

Uprawnienia do us�ug sieciowych ePUAP nadaje Administrator podmiotu, szczeg�owe informacje w instrukcji dla �rodowiska produkcyjnego:

epuap.gov.pl>>STREFA URZ�DNIKA>>Dla integrator�w>>Integracja>>Konfiguracja w zakresie integracji

Adres konsoli Draco dla �rodowiska integracyjnego:

https://konsolahetman-int.epuap.gov.pl/DracoConsole

Certyfikaty do weryfikacji odpowiedzi umie�cili�my na int.epuap.gov.pl>>POMOC>>Informacja o �rodowisku

Opis Us�ug sieciowych ePUAP znajduje si� w instrukcji: : epuap.gov.pl>>STREFA URZ�DNIKA>>Dla integrator�w>>Specyfikacja WSDL>>ePUAP - Dokumentacja us�ug oraz

epuap.gov.pl>>STREFA URZ�DNIKA>>Dla integrator�w>>Specyfikacja WSDL>>Instrukcja integratora - obs�uga du�ych plik�w w ePUAP2

Us�ugi sieciowe ePUAP dzia�aj� analogicznie jak aplikacja ePUAP. Podstawowe informacje na temat dost�pnych tryb�w przesy�ania pism, zale�no�ci pomi�dzy ustawionymi parametrami skrytki, weryfikacji poprawno�ci przesy�ania pism znajduj� si� w instrukcji:

epuap.gov.pl>>STREFA URZ�DNIKA>>Instrukcje i podr�czniki>> Administrowanie kontem podmiotu publicznego

Do wysy�ania pism zosta�y przygotowane dwa Webservices:

WS-Skrytka s�u�y do wysy�ki pism w trybie UPP
WS-Doreczyciel s�u�y do wysy�ki pism w trybie UPD.
Do odbierania pism ze skrytki s�u�y WS-pull

Przez ePUAP mo�na przesy�a� pisma, kt�re s� poprawnymi xml�ami i s� oparte o wzory opublikowane w CRWD. Zasady jakie powinny spe�nia� wzory oraz formularze opisane s� na stronie Ministerstwa Cyfryzacji:

http://mc.bip.gov.pl/centralne-repozytorium-wzorow-dokumentow-elektronicznych/43003_centralne-repozytorium-wzorow-dokumentow-elektronicznych.html