# Instrukcja generowania certyfikatu na œrodowisku testowym, skopiowana z dokumentu 

https://epuap.gov.pl/wps/wcm/connect/92c64f1f-081e-4641-8bc8-651cb8205fac/ePUAP+-+Instrukcja+generowania+%C5%BC%C4%85dania+certyfikatu+do+komunikacji+z+ePUAP+oraz+PZ_v1.1.pdf?MOD=AJPERES

Instrukcja na stronie ePUAP, po zalogowaniu, w opisie procesu **Wniosek o certyfikat do œrodowiska integracyjnego ePUAP/PZ**

```
Aby z³o¿yæ wniosek o certyfikat musisz byæ zalogowany do podmiotu, który ma nadan¹ rolê Instytucja publiczna w œrodowisku int.epuap.gov.pl. 
Jeœli nie masz za³o¿onego takiego podmiotu a pracujesz w podmiocie publicznym lub pracujesz w firmie, która tworzy integracje dla podmiotów 
publicznych to mo¿esz utworzyæ podmiot publiczny w œrodowisku int.epuap.gov.pl. Po utworzeniu podmiotu wyœlij mail 
na adres int-epuap-pomoc@coi.gov.pl z proœb¹ o nadanie roli Instytucja publiczna. W mailu podaj ID podmiotu, który utworzy³eœ. 
Wniosek o certyfikat bêdziesz móg³ z³o¿yæ gdy otrzymasz od nas informacjê, ¿e nadaliœmy Twojej organizacji rolê Instytucji publicznej.
```

* Krok 1. Utworzenie keystore - CreateKeyStore.bat, has³o Demo1234
* Krok 2. Utworzenie ¿¹dania  - CreateCSR.bat

* Krok 3. To co przyœl¹ nale¿y po³¹czyæ do keystore - Import.bat

-------------------------------------------------

Na stronie generowania zaznaczam te¿ ¿e maj¹ byæ inne us³ugi i oni pisz¹ tak

Uprawnienia do us³ug sieciowych ePUAP nadaje Administrator podmiotu, szczegó³owe informacje w instrukcji dla œrodowiska produkcyjnego:

epuap.gov.pl>>STREFA URZÊDNIKA>>Dla integratorów>>Integracja>>Konfiguracja w zakresie integracji

Adres konsoli Draco dla œrodowiska integracyjnego:

https://konsolahetman-int.epuap.gov.pl/DracoConsole

Certyfikaty do weryfikacji odpowiedzi umieœciliœmy na int.epuap.gov.pl>>POMOC>>Informacja o œrodowisku

Opis Us³ug sieciowych ePUAP znajduje siê w instrukcji: : epuap.gov.pl>>STREFA URZÊDNIKA>>Dla integratorów>>Specyfikacja WSDL>>ePUAP - Dokumentacja us³ug oraz

epuap.gov.pl>>STREFA URZÊDNIKA>>Dla integratorów>>Specyfikacja WSDL>>Instrukcja integratora - obs³uga du¿ych plików w ePUAP2

Us³ugi sieciowe ePUAP dzia³aj¹ analogicznie jak aplikacja ePUAP. Podstawowe informacje na temat dostêpnych trybów przesy³ania pism, zale¿noœci pomiêdzy ustawionymi parametrami skrytki, weryfikacji poprawnoœci przesy³ania pism znajduj¹ siê w instrukcji:

epuap.gov.pl>>STREFA URZÊDNIKA>>Instrukcje i podrêczniki>> Administrowanie kontem podmiotu publicznego

Do wysy³ania pism zosta³y przygotowane dwa Webservices:

WS-Skrytka s³u¿y do wysy³ki pism w trybie UPP
WS-Doreczyciel s³u¿y do wysy³ki pism w trybie UPD.
Do odbierania pism ze skrytki s³u¿y WS-pull

Przez ePUAP mo¿na przesy³aæ pisma, które s¹ poprawnymi xml’ami i s¹ oparte o wzory opublikowane w CRWD. Zasady jakie powinny spe³niaæ wzory oraz formularze opisane s¹ na stronie Ministerstwa Cyfryzacji:

http://mc.bip.gov.pl/centralne-repozytorium-wzorow-dokumentow-elektronicznych/43003_centralne-repozytorium-wzorow-dokumentow-elektronicznych.html