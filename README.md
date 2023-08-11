# Otron - Projekt mobilnej gry multiplayer
Wykonana aplikacja na silniku Unity pozwala na grę w trybie rywalizacji dwóch osób łącząc się ze sobą poprzez Wi-Fi gdy znajdują się w tej samej sieci. Gra jest opatrzona w intuicyjny i przejrzysty interfejs graficzny umożliwiający swobodne korzystanie z wszystkich funkcjonalności jakie oferuje aplikacja. Sterowanie odbywa się za pomocą joystick'a oraz joybutton'a w oparciu o ekran dotykowy danego urządzenia. Gra jest osadzona w futurystycznym klimacie, w której to celem jest celu trafienie przeciwnika pociskiem  i tym samym skucie jego życia, unikając przy tym pocisków wystrzelonych przez naszego oponenta. Gra kończy się w momencie gdy jeden z graczy straci wszystkie swoje życia. Aplikacja ma na celu zapewnienia rozrywki użytkownikom.

## Cel projektu
Zaprojektowanie i wykonanie oprogramowania umożliwiającego połączenia ze sobą dwóch użytkowników znajdujących się w tej samej sieci LAN, w celu wspólnej i równoległej gry w trybie rywalizacji na jednej mapie, z dwóch różnych urządzeń opartych na systemie Android.

## Cele w rozgrywce
* Celem gry/ warunkiem wygranej jest skucie gracza grającego przeciw nam na drugim końcu mapy trzykrotnie oddając w jego kierunku celny strzał, który skoliduje z obiektem reprezentującym gracza-oponenta.
* Podczas rozgrywki trzeba uważać na odbijające się pociski od ścian, w związku z ciągłym ruchem ścian jest ciężkie do przewidzenia, w którym kierunku odbije się pocisk.
* Trzeba również uważać na trudności środowiska, między innymi na działka, które będą próbowały skuć graczy po obu stronach.

## Przykładowe zrzuty ekranu
![Screenshot-1](/screens/3.jpg)
![Screenshot-1](/screens/9.jpg)
![Screenshot-1](/screens/6.jpg)

## Wymagania funkcjonalne i niefunkcjonalne aplikacji
* Gra w trybie rywalizacji,
* Interaktywne UI oparte o przyciski,
* Możliwość „Hostowania” gry jak i dołączania do istniejącego w sieci serwera,
* Automatyczne wyszukiwanie dostępnego serwera w sieci LAN (Network Discovery),
* Lobby umożliwiające równoczesne wystartowanie gry na obu urządzeniach,
* Kontrola graczy za pomocą joystick'a (poruszanie się, oddanie strzału),
* Możliwość gry z muzyką i dźwiękami lub bez.
* System żyć, definiujący kto wygrał daną rozgrywkę,
* Graficzne liczniki przeładowania graczy po oddaniu strzału,
* Dostępne dodatkowe tryby gry tj. Friendly Fire ON/OFF, NightMode ON/OFF

## Narzędzie i technologie
* Projekt gry wykonywany na silniku **Unity**. 
* Do zarządzania funkcjonalnościami związanymi z siecią/parowaniem graczy wykorzystany został „high level Networking API” **Mirror Networking**. 
* Skrypty pisane w **Visual Studio Code**, wykorzystując język **C#**. 
* Do wykonania modeli graficznych na potrzeby projektu wykorzystany został **Adobe Photoshop CC 2019**. 
* Oprogramowanie jakie zostało wykorzystane do edycji plików audio to **Audacity**. 
* Wszelkie efekty wizualne oparte zostały na **Particle System**, jak i również w małej części na **Shader Graph**, oba te narzędzia są dostępne w edytorze Unity.   
  
## Wymagania
**Wymagania systemowe:**  
* System operacyjny :  Android 4.4 (API 19)+
* CPU: ARMv7 with Neon Support (32-bit) or ARM64
* Graphics API: OpenGL ES 2.0+, OpenGL ES 3.0+, Vulkan  
  
**Wymagania sprzętowe:**
* RAM: 2GB + 
* Procesor: HiSilicon Kirin 659 - 4 x 2.36GHz & 4 x 1.7GHz + 
* Procesor graficzny: ARM Mali-T830 MP2 @900 MHz +
* Wolna pamięć: 60 MB + 

## GUI
### Main menu: 
Przyciski z załączonymi funkcjami wykonującymi się przy zdarzeniu „onClick”. Dostępne przyciski to „Play” – przełącza nas na nową scenę - do lobby, „Options” – pozwala na dostosowanie poziomu głośności audio, „Quit” – pozwala na zamknięcie aplikacji.

### Lobby: 
Również wykorzystane przyciski opatrzone zdarzeniami „onClick”. Dostępne przyciski to „Host”- przycisk odpowiadający za utworzenie „pokoju” rozgrywki, osoba która hostuje grę oznacza iż jest ona jednocześnie serwerem jak i klientem, „Join”- pozwala na wyszukanie dostępnych serwerów w sieci LAN, jeśli jest jakiś dostępny pozwala dołączyć do pokoju rozgrywki wystartowanego na innym urządzeniu za pomocą przycisku „host”, „Return” – przycisk pozwalający na powrót do main menu,
„Night mode”/”Friendly fire mode” – przyciski dostępne tylko na serwerze za pomocą których można włączyć dodatkowe tryby gry, „Strat”- przycisk dostępny tylko na serwerze  pozwala na rozpoczęcie rozgrywki, w momencie gdy w lobby jest dwóch graczy.

### Win/Lost menu: 
Zastosowanie dwóch przycisków takich jak „Play again” – powrót do Lobby z możliwością rozpoczęcia kolejnej rozgrywki, „Main menu” – powrót do main menu.

### Joystick
Dynamiczny przycisk przypominający „drążek sterowniczy”- służy do kontroli obiektem gracza, umożliwia poruszanie się w dowolnym kierunku, pojawia się w miejscu pierwszego zarejestrowanego dotyku i zakotwicza go w tym miejscu do momentu aż zdejmiemy palec z tego miejsca.

### Joybutton
Przycisk uruchamiający sekwencję przy zdarzeniu „onClick” odpowiedzialną za strzelanie pociskami.

## Struktura programu
### Skrypty/klasy zarządzające działaniem aplikacji:
- MainMenu – zawiera zestaw funkcji do przycisków pojawiających się w main menu
- SceneTransitionCanvas – uruchamia animację reprezentujące płynne przejścia pomiędzy scenami
- LevelManager – zawiera zestaw „ogólnych” zmiennych jak i  funkcji, do których często się odwołuje z pozycji innych klas.
- NetworkManagerProjekt, ServerResponse, ServerRequest, NetworkBehaviour – zarządzanie zdarzeniami związanymi z siecią/połączeniem, umożliwia implementację funkcjonalności związanych z „multiplayerem”.
- NetworkDiscovery – umożliwia wychwycenie dostępnych serwerów w sieci LAN i pobrania ich adresów IP, w celu późniejszego nawiązania połączenia z nimi.
- Connect – zestaw funkcji dla przycisków z sekcji lobby (hostowanie, wyszukiwanie dostępnych serwerów oraz łączenie się z nimi)
- LobbyCanvas – zestaw funkcji zarządzających ustawieniami rozgrywki, równoległego wystartowania gry na obu urządzeniach, zarządzanie obiektami znajdującymi się w sekcji lobby.
- PlayerControl- skrypt zaczepiony do obiektu gracza, pozwala na poruszanie się, dokonywanie strzału, detekcja kolizji..
- LifeSystem – zawiera mechanizm żyć, pozwala na dodawanie/odejmowanie ilości żyć przy określonych zdarzeniach
- JoyButton – wyczuwanie czy przycisk strzału został kliknięty
- Joystick – wyczuwanie czy przycisk poruszania został kliknięty oraz w jakim kierunku powinien poruszyć się gracz
- CannonRotating – zarządza całą mechaniką działek
- CannonBullet – definiuje prędkość i kierunek pocisku
- CineMachineShake – zarządza efektem „screen shake”, przy określonych zdarzeniach
- MenuCannon – definiuje zachowanie działka znajdującego się w sekcji main menu
- WhirlingBlock – zarządza obracającym się obiektem-panelem w grze, określa jego prędkość i sposób rotacji
- PopupCatcher – uruchamia określone komunikaty na podstawie wartości pewnych zmiennych 
- GameModeSettings – zawiera zmienne definiujące jaki tryb gry z poziomu lobby został wybrany
- CameraEdge – pozwala na dynamiczne dostosowywanie bloków kolizyjnych do rozmiaru ekranu
- BulletColisionCounter – zarządza mechaniką pocisków wystrzeliwanych przez graczy
- Oraz inne (CenterBlockHandler, CheckOnstart,DisconnectionPopupHandler, Okbutton, ServerButton, WallHandler, PowerBar...)









