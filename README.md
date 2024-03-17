Solution to the test task for junior developer position at Showtime VR
----------------------------------------------------------------------
The task was modified for completing in Unity 
----------------------------------------------------------------------
Przy uruchomieniu aplikacja wczytuje pliki i urządzenia i wyświetla aktualny stan.
Kliknięcie w kafelek pliku na liście wszystkich plików powoduje jego zaznaczenie. W tym samym
czasie może być zaznaczonych kilka plików. Nie można odznaczyć pliku. Zaznaczenie pliku
powoduje dodanie go do urządzeń które go jeszcze nie mają i zapisanie zmian w backend.
Nowy plik dodany do urządzenia ma postęp ściągania 0. Urządzenie ściąga plik z prędkością
określoną dla danego urządzenia. Dane urządzenie w danym czasie może ściągać tylko 1 plik. Dany
plik w danym czasie może być ściągany przez tylko 1 urządzenie. Ukończenie ściągania pliku
powoduje zapisanie zmian w backend.
Pierwsza kolumna to lista wszystkich plików. Zaznaczony przez użytkownika plik wyświetla się jako
czarny.
Kolejne kolumny to urządzenia i znajdujące się na nich pliki. Kafelek pliku na urządzeniu wyświetla
postęp ściągania w formie paska.

Model
 - files - lista obiektów File
 - devices - lista obiektów Device

File
 - id
 - name
 - size - rozmiar pliku (bytes)

Device
 - id
 - name
 - download - prędkość ściągania (bytes/second)
 - files - lista obiektów DeviceFile przechowujących stan plików na urządzeniu

DeviceFile
 - id - odpowiadające File.id
 - progress - postęp ściągania pliku (0-1)
