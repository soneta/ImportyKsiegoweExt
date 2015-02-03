
# ImportyKsiegoweExt

Niniejsze repozytorium zawiera przykładowy dodatek do enova365 oparty o bibliotekę Enova.ImportyKsiegowe, który ilustruje implementację importu danych księgowych z pliku tekstowego o określonej strukturze.

Przykładowy dodatek pozwala na import z pliku CSV:

- kontrahentów
- ewidencji raportu bankowego
- zapłat do ewidencji raportu bankowego

Aby skorzystać z zamieszczonego przykładu należy posiadać:

- zainstalowaną aplikację enova365
- środowisko programistyczne Visual Studio 2013

> Zakłada się, że korzystający z przykładu "GitHubExampleImportTxt" zna:
<br/>- aplikację **enova365**
<br/>- dodatek **Enova.ImportyKsiegowe**
<br/>- środowisko programistyczne **Visual Studio**.

## Zawartość

- Folder "GitHubExampleImportTxt" - kod dodatku dla Visual Studio 2013 (C#)
- Folder "GitHubExampleImportTxt.Inne" - dodatkowe pliki wykorzystywane w przykładzie

## Instrukcja

Proponowane wykorzystanie niniejszego repozytorium obejmuje następujące kroki:

1. Pobrać zawartość repozytorium
2. Zapoznać się ze strukturą przykładowego pliku "SampleInput.txt"
3. Skonfigurować bazę demo, do której zostaną zaimportowane dane
4. Skompilować przykładowy dodatek i podpiąć go do aplikacji enova365
5. Zaimportować dane z pliku "SampleInput.txt" i zweryfikować poprawność importu

### Pobranie repozytorium

Repozytorium dostępne jest pod adresem  
[https://github.com/soneta/ImportyKsiegoweExt.git](https://github.com/soneta/ImportyKsiegoweExt.git)

### Struktura importowanego pliku

Plikiem wejściowym jest plik tesktowy CSV. Każdy wiersz zawiera jedno polecenie importu. Pola w wierszu oddzielone są znakiem średnika. Pierwsze pole oznacza typ rekordu:

- **KON**: dane kontrahenta;  
(kolejne pola oznaczają: kod i nazwę)

- **ESP**: wskazanie docelowej ewidencji środków pieniężnych wg podanego symbolu  
(dane nie są wprowadzane - ustawiana jest tylko informacja kontekstowa)
 
- **RAP**: dane ewidencji raportu bankowego  
(kolejne pola oznaczają: symbol definicji raportu, datę raportu, numer raportu)

- **WPL**, **WYP**: dane wpłaty/wypłaty dodawanej do raportu  
(kolejne pola oznaczają: datę zapłaty, kod kontrahenta, sposób zapłaty, kwotę, symbol kwoty, numer zapłaty, opis zapłaty)

### Przygotowanie bazy demo

Punktem wyjścia do przeprowadzenia importu jest baza demo "złota", do której należy zaimportować plik xml o nazwie "EnovaConfig.xml". W pliku tym zawarta jest definicja rachunku bankowego o symbolu "BANK7", do której zostaną wprowadzone dane.

Po zaimportowaniu pliku XML należy w ustawieniach zaimportowanego rachunku bankowego przydzielić uprawnienia operatorowi *Administrator* (bezpośrednio po imporcie domyślnie operatorzy mają zakaz dostępu do EŚP).

### Kompilacja dodatku

Przed skompilowaniem pobranego kodu konieczna będzie zmiana niektórych ustawień projektu Visual Studio.

#### Referencje do zewnętrznych bibliotek

Dodatek zawiera referencje do następujących bibliotek (poza systemowymi):

- Soneta.Business
- Soneta.Core
- Soneta.EI
- Soneta.Type
- Enova.ImportyKsiegowe

Biblioteka *Enova.ImportyKsiegowe* dołączona jest do projektu. Lokalizacja bibliotek *Soneta.** należy wskazać odpowiednio do konfiguracji własnego systemu.

Ustawienie w projekcie VS: **Reference Paths** 

#### Docelowy katalog kompilacji

W projekcie ustawiony jest folder "C:\Program Files (x86)\Common Files\Soneta\Assemblies\". Zapewnia to, że wynikowa biblioteka zostanie od razu umieszczona w katalogu dodatków dla wersji enova365 32-bitowej.
> Przed kompilacją należy upewnić się, że bieżący operator Windows ma prawo zapisu do w/w folderu.

Ustawienie w projekcie VS: **Build / Output / Output path**.

#### Podpięcie dodatku do enova365

Przed uruchomieniem enova365 należy upewnić się, że ma ona dostęp do potrzebnych dodatków:

- Enova.ImportyKsiegowe: dodatek standardowy
- GitHubExampleImportTxt: niniejszy dodatek

Dodatki należy umieścić w jednym z folderów w zależności od tego czy uruchamiamy wersję 32 czy 64 bitową enova:

- 32bit: C:\Program Files (x86)\Common Files\Soneta\Assemblies\
- 64bit: C:\Program Files\Common Files\Soneta\Assemblies\

### Import pliku

Po uruchomieniu enova365 opcja importu dostępna będzie z menu PLIK/IMPORTUJ ZAPISY pod nazwą **IMPORT TXT PRZYKŁAD Z GITHUB...**

Po poprawnym imporcie danych z przykładowego pliku "SampleInput.txt" w aplikacji powinny pojawić się następujące dane:

- kontrahenci o kodach KOWALSKA, KOWALSKI
- dwie ewidencje typu raport bankowy z 5 i 6 lutego 2015
- cztery zapłaty umieszczone w w/w ewidencjach 