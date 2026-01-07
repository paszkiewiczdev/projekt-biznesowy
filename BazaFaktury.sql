USE Faktury;
GO

CREATE TABLE SposobPlatnosci (
    IdSposobuPlatnosci INT IDENTITY(1,1) PRIMARY KEY,
    Nazwa NVARCHAR(100) NOT NULL,
    Opis NVARCHAR(255) NULL,
    CzyAktywny BIT NOT NULL DEFAULT 1
);

CREATE TABLE StatusFaktury (
    IdStatusuFaktury INT IDENTITY(1,1) PRIMARY KEY,
    Nazwa NVARCHAR(50) NOT NULL,
    Opis NVARCHAR(255) NULL,
    CzyAktywny BIT NOT NULL DEFAULT 1
);

CREATE TABLE KategoriaTowaru (
    IdKategoriiTowaru INT IDENTITY(1,1) PRIMARY KEY,
    Nazwa NVARCHAR(100) NOT NULL,
    Opis NVARCHAR(255) NULL,
    CzyAktywny BIT NOT NULL DEFAULT 1
);

CREATE TABLE JednostkaMiary (
    IdJednostkiMiary INT IDENTITY(1,1) PRIMARY KEY,
    Symbol NVARCHAR(20) NOT NULL,
    Nazwa NVARCHAR(50) NOT NULL,
    CzyAktywna BIT NOT NULL DEFAULT 1
);

CREATE TABLE Waluta (
    IdWaluty INT IDENTITY(1,1) PRIMARY KEY,
    Kod NVARCHAR(3) NOT NULL,
    Nazwa NVARCHAR(50) NOT NULL,
    Kurs DECIMAL(18,4) NULL,
    CzyDomyslna BIT NOT NULL DEFAULT 0,
    CzyAktywna BIT NOT NULL DEFAULT 1
);

CREATE TABLE StawkaVat (
    IdStawkiVat INT IDENTITY(1,1) PRIMARY KEY,
    Nazwa NVARCHAR(20) NOT NULL,
    Wartosc DECIMAL(5,2) NOT NULL,
    CzyAktywna BIT NOT NULL DEFAULT 1
);

CREATE TABLE TypKontrahenta (
    IdTypuKontrahenta INT IDENTITY(1,1) PRIMARY KEY,
    Nazwa NVARCHAR(50) NOT NULL,
    CzyAktywny BIT NOT NULL DEFAULT 1
);

CREATE TABLE StatusZamowienia (
    IdStatusuZamowienia INT IDENTITY(1,1) PRIMARY KEY,
    Nazwa NVARCHAR(50) NOT NULL,
    CzyAktywny BIT NOT NULL DEFAULT 1
);

CREATE TABLE Magazyn (
    IdMagazynu INT IDENTITY(1,1) PRIMARY KEY,
    Kod NVARCHAR(20) NOT NULL,
    Nazwa NVARCHAR(100) NOT NULL,
    Opis NVARCHAR(255) NULL,
    CzyAktywny BIT NOT NULL DEFAULT 1
);

CREATE TABLE TypDokumentuMagazynowego (
    IdTypuDokumentu INT IDENTITY(1,1) PRIMARY KEY,
    Kod NVARCHAR(10) NOT NULL,
    Nazwa NVARCHAR(50) NOT NULL,
    CzyAktywny BIT NOT NULL DEFAULT 1
);

CREATE TABLE Kontrahent (
    IdKontrahenta INT IDENTITY(1,1) PRIMARY KEY,
    IdTypuKontrahenta INT NULL,
    NIP NVARCHAR(20) NULL,
    NazwaPelna NVARCHAR(200) NOT NULL,
    NazwaSkrocona NVARCHAR(100) NULL,
    Telefon NVARCHAR(30) NULL,
    Email NVARCHAR(100) NULL,
    CzyAktywny BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (IdTypuKontrahenta) REFERENCES TypKontrahenta(IdTypuKontrahenta)
);

CREATE TABLE AdresKontrahenta (
    IdAdresu INT IDENTITY(1,1) PRIMARY KEY,
    IdKontrahenta INT NOT NULL,
    Ulica NVARCHAR(100) NULL,
    NrDomu NVARCHAR(20) NULL,
    NrLokalu NVARCHAR(20) NULL,
    KodPocztowy NVARCHAR(10) NULL,
    Miasto NVARCHAR(100) NULL,
    Kraj NVARCHAR(100) NULL,
    CzyDomyslny BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (IdKontrahenta) REFERENCES Kontrahent(IdKontrahenta)
);

CREATE TABLE Towar (
    IdTowaru INT IDENTITY(1,1) PRIMARY KEY,
    Kod NVARCHAR(50) NOT NULL,
    Nazwa NVARCHAR(150) NOT NULL,
    IdKategoriiTowaru INT NULL,
    IdJednostkiMiary INT NULL,
    IdStawkiVatSprzedazy INT NULL,
    IdStawkiVatZakupu INT NULL,
    Cena DECIMAL(18,2) NOT NULL DEFAULT 0,
    Marza DECIMAL(18,2) NULL,
    CzyAktywny BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (IdKategoriiTowaru) REFERENCES KategoriaTowaru(IdKategoriiTowaru),
    FOREIGN KEY (IdJednostkiMiary) REFERENCES JednostkaMiary(IdJednostkiMiary),
    FOREIGN KEY (IdStawkiVatSprzedazy) REFERENCES StawkaVat(IdStawkiVat),
    FOREIGN KEY (IdStawkiVatZakupu) REFERENCES StawkaVat(IdStawkiVat)
);

CREATE TABLE FakturaSprzedazy (
    IdFakturySprzedazy INT IDENTITY(1,1) PRIMARY KEY,
    Numer NVARCHAR(50) NOT NULL,
    DataWystawienia DATE NOT NULL,
    DataSprzedazy DATE NOT NULL,
    IdKontrahenta INT NOT NULL,
    IdSposobuPlatnosci INT NULL,
    IdStatusuFaktury INT NULL,
    IdWaluty INT NULL,
    TerminPlatnosci DATE NULL,
    RazemNetto DECIMAL(18,2) NULL,
    RazemVat DECIMAL(18,2) NULL,
    RazemBrutto DECIMAL(18,2) NULL,
    FOREIGN KEY (IdKontrahenta) REFERENCES Kontrahent(IdKontrahenta),
    FOREIGN KEY (IdSposobuPlatnosci) REFERENCES SposobPlatnosci(IdSposobuPlatnosci),
    FOREIGN KEY (IdStatusuFaktury) REFERENCES StatusFaktury(IdStatusuFaktury),
    FOREIGN KEY (IdWaluty) REFERENCES Waluta(IdWaluty)
);

CREATE TABLE PozycjaFakturySprzedazy (
    IdPozycjiFakturySprzedazy INT IDENTITY(1,1) PRIMARY KEY,
    IdFakturySprzedazy INT NOT NULL,
    IdTowaru INT NOT NULL,
    Ilosc DECIMAL(18,3) NOT NULL,
    CenaNetto DECIMAL(18,2) NOT NULL,
    IdStawkiVat INT NULL,
    WartoscNetto DECIMAL(18,2) NULL,
    WartoscVat DECIMAL(18,2) NULL,
    WartoscBrutto DECIMAL(18,2) NULL,
    FOREIGN KEY (IdFakturySprzedazy) REFERENCES FakturaSprzedazy(IdFakturySprzedazy),
    FOREIGN KEY (IdTowaru) REFERENCES Towar(IdTowaru),
    FOREIGN KEY (IdStawkiVat) REFERENCES StawkaVat(IdStawkiVat)
);

CREATE TABLE ZamowienieSprzedazy (
    IdZamowieniaSprzedazy INT IDENTITY(1,1) PRIMARY KEY,
    Numer NVARCHAR(50) NOT NULL,
    DataZamowienia DATE NOT NULL,
    IdKontrahenta INT NOT NULL,
    IdStatusuZamowienia INT NULL,
    Uwagi NVARCHAR(255) NULL,
    FOREIGN KEY (IdKontrahenta) REFERENCES Kontrahent(IdKontrahenta),
    FOREIGN KEY (IdStatusuZamowienia) REFERENCES StatusZamowienia(IdStatusuZamowienia)
);

CREATE TABLE PozycjaZamowieniaSprzedazy (
    IdPozycjiZamowieniaSprzedazy INT IDENTITY(1,1) PRIMARY KEY,
    IdZamowieniaSprzedazy INT NOT NULL,
    IdTowaru INT NOT NULL,
    Ilosc DECIMAL(18,3) NOT NULL,
    CenaNetto DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (IdZamowieniaSprzedazy) REFERENCES ZamowienieSprzedazy(IdZamowieniaSprzedazy),
    FOREIGN KEY (IdTowaru) REFERENCES Towar(IdTowaru)
);

CREATE TABLE StanMagazynowy (
    IdStanuMagazynowego INT IDENTITY(1,1) PRIMARY KEY,
    IdMagazynu INT NOT NULL,
    IdTowaru INT NOT NULL,
    Ilosc DECIMAL(18,3) NOT NULL DEFAULT 0,
    FOREIGN KEY (IdMagazynu) REFERENCES Magazyn(IdMagazynu),
    FOREIGN KEY (IdTowaru) REFERENCES Towar(IdTowaru)
);

CREATE TABLE DokumentMagazynowy (
    IdDokumentuMagazynowego INT IDENTITY(1,1) PRIMARY KEY,
    Numer NVARCHAR(50) NOT NULL,
    DataDokumentu DATE NOT NULL,
    IdMagazynu INT NOT NULL,
    IdTypuDokumentu INT NOT NULL,
    Uwagi NVARCHAR(255) NULL,
    FOREIGN KEY (IdMagazynu) REFERENCES Magazyn(IdMagazynu),
    FOREIGN KEY (IdTypuDokumentu) REFERENCES TypDokumentuMagazynowego(IdTypuDokumentu)
);

CREATE TABLE PozycjaDokumentuMagazynowego (
    IdPozycjiDokumentuMagazynowego INT IDENTITY(1,1) PRIMARY KEY,
    IdDokumentuMagazynowego INT NOT NULL,
    IdTowaru INT NOT NULL,
    Ilosc DECIMAL(18,3) NOT NULL,
    FOREIGN KEY (IdDokumentuMagazynowego) REFERENCES DokumentMagazynowy(IdDokumentuMagazynowego),
    FOREIGN KEY (IdTowaru) REFERENCES Towar(IdTowaru)
);
