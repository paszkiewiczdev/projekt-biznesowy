USE Faktury;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM SposobPlatnosci WHERE Nazwa = 'TEST_Przelew')
    BEGIN
        INSERT INTO SposobPlatnosci (Nazwa, Opis, CzyAktywny)
        VALUES ('TEST_Przelew', 'Testowy przelew bankowy', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM SposobPlatnosci WHERE Nazwa = 'TEST_Gotowka')
    BEGIN
        INSERT INTO SposobPlatnosci (Nazwa, Opis, CzyAktywny)
        VALUES ('TEST_Gotowka', 'Testowa płatność gotówką', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM StatusFaktury WHERE Nazwa = 'TEST_Wystawiona')
    BEGIN
        INSERT INTO StatusFaktury (Nazwa, Opis, CzyAktywny)
        VALUES ('TEST_Wystawiona', 'Testowy status: wystawiona', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM StatusFaktury WHERE Nazwa = 'TEST_Oplacona')
    BEGIN
        INSERT INTO StatusFaktury (Nazwa, Opis, CzyAktywny)
        VALUES ('TEST_Oplacona', 'Testowy status: opłacona', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM KategoriaTowaru WHERE Nazwa = 'TEST_Elektronika')
    BEGIN
        INSERT INTO KategoriaTowaru (Nazwa, Opis, CzyAktywny)
        VALUES ('TEST_Elektronika', 'Testowa kategoria towaru', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM KategoriaTowaru WHERE Nazwa = 'TEST_Usługi')
    BEGIN
        INSERT INTO KategoriaTowaru (Nazwa, Opis, CzyAktywny)
        VALUES ('TEST_Usługi', 'Testowa kategoria usług', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM JednostkaMiary WHERE Symbol = 'TEST_szt')
    BEGIN
        INSERT INTO JednostkaMiary (Symbol, Nazwa, CzyAktywna)
        VALUES ('TEST_szt', 'Testowa sztuka', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM JednostkaMiary WHERE Symbol = 'TEST_usl')
    BEGIN
        INSERT INTO JednostkaMiary (Symbol, Nazwa, CzyAktywna)
        VALUES ('TEST_usl', 'Testowa usługa', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM Waluta WHERE Kod = 'TEST_PLN')
    BEGIN
        INSERT INTO Waluta (Kod, Nazwa, Kurs, CzyDomyslna, CzyAktywna)
        VALUES ('TEST_PLN', 'Testowy PLN', 1.0000, 0, 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM Waluta WHERE Kod = 'TEST_EUR')
    BEGIN
        INSERT INTO Waluta (Kod, Nazwa, Kurs, CzyDomyslna, CzyAktywna)
        VALUES ('TEST_EUR', 'Testowy EUR', 4.3000, 0, 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM StawkaVat WHERE Nazwa = 'TEST_VAT23')
    BEGIN
        INSERT INTO StawkaVat (Nazwa, Wartosc, CzyAktywna)
        VALUES ('TEST_VAT23', 23.00, 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM StawkaVat WHERE Nazwa = 'TEST_VAT8')
    BEGIN
        INSERT INTO StawkaVat (Nazwa, Wartosc, CzyAktywna)
        VALUES ('TEST_VAT8', 8.00, 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM TypKontrahenta WHERE Nazwa = 'TEST_Firma')
    BEGIN
        INSERT INTO TypKontrahenta (Nazwa, CzyAktywny)
        VALUES ('TEST_Firma', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM TypKontrahenta WHERE Nazwa = 'TEST_Osoba')
    BEGIN
        INSERT INTO TypKontrahenta (Nazwa, CzyAktywny)
        VALUES ('TEST_Osoba', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM StatusZamowienia WHERE Nazwa = 'TEST_Nowe')
    BEGIN
        INSERT INTO StatusZamowienia (Nazwa, CzyAktywny)
        VALUES ('TEST_Nowe', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM StatusZamowienia WHERE Nazwa = 'TEST_WRealizacji')
    BEGIN
        INSERT INTO StatusZamowienia (Nazwa, CzyAktywny)
        VALUES ('TEST_WRealizacji', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM Magazyn WHERE Kod = 'TEST_MAG1')
    BEGIN
        INSERT INTO Magazyn (Kod, Nazwa, Opis, CzyAktywny)
        VALUES ('TEST_MAG1', 'TEST Magazyn Główny', 'Testowy magazyn główny', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM Magazyn WHERE Kod = 'TEST_MAG2')
    BEGIN
        INSERT INTO Magazyn (Kod, Nazwa, Opis, CzyAktywny)
        VALUES ('TEST_MAG2', 'TEST Magazyn Pomocniczy', 'Testowy magazyn pomocniczy', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM TypDokumentuMagazynowego WHERE Kod = 'TEST_PZ')
    BEGIN
        INSERT INTO TypDokumentuMagazynowego (Kod, Nazwa, CzyAktywny)
        VALUES ('TEST_PZ', 'TEST Przyjęcie zewnętrzne', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM TypDokumentuMagazynowego WHERE Kod = 'TEST_WZ')
    BEGIN
        INSERT INTO TypDokumentuMagazynowego (Kod, Nazwa, CzyAktywny)
        VALUES ('TEST_WZ', 'TEST Wydanie zewnętrzne', 1);
    END;

    DECLARE @IdTypuKontrahentaFirma INT = (
        SELECT TOP 1 IdTypuKontrahenta FROM TypKontrahenta WHERE Nazwa = 'TEST_Firma'
    );
    DECLARE @IdTypuKontrahentaOsoba INT = (
        SELECT TOP 1 IdTypuKontrahenta FROM TypKontrahenta WHERE Nazwa = 'TEST_Osoba'
    );

    IF NOT EXISTS (SELECT 1 FROM Kontrahent WHERE NazwaPelna = 'TEST_Kontrahent_Alfa')
    BEGIN
        INSERT INTO Kontrahent (IdTypuKontrahenta, NIP, NazwaPelna, NazwaSkrocona, Telefon, Email, CzyAktywny)
        VALUES (@IdTypuKontrahentaFirma, 'TEST_NIP_111', 'TEST_Kontrahent_Alfa', 'TEST_Alfa', '111-111-111', 'test.alfa@example.com', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM Kontrahent WHERE NazwaPelna = 'TEST_Kontrahent_Beta')
    BEGIN
        INSERT INTO Kontrahent (IdTypuKontrahenta, NIP, NazwaPelna, NazwaSkrocona, Telefon, Email, CzyAktywny)
        VALUES (@IdTypuKontrahentaOsoba, 'TEST_NIP_222', 'TEST_Kontrahent_Beta', 'TEST_Beta', '222-222-222', 'test.beta@example.com', 1);
    END;

    DECLARE @IdKontrahentaAlfa INT = (
        SELECT TOP 1 IdKontrahenta FROM Kontrahent WHERE NazwaPelna = 'TEST_Kontrahent_Alfa'
    );
    DECLARE @IdKontrahentaBeta INT = (
        SELECT TOP 1 IdKontrahenta FROM Kontrahent WHERE NazwaPelna = 'TEST_Kontrahent_Beta'
    );

    IF NOT EXISTS (SELECT 1 FROM AdresKontrahenta WHERE Ulica = 'TEST_Ulica_Alfa')
    BEGIN
        INSERT INTO AdresKontrahenta (IdKontrahenta, Ulica, NrDomu, NrLokalu, KodPocztowy, Miasto, Kraj, CzyDomyslny)
        VALUES (@IdKontrahentaAlfa, 'TEST_Ulica_Alfa', '1', '1', '00-001', 'TEST_Warszawa', 'TEST_Polska', 1);
    END;

    IF NOT EXISTS (SELECT 1 FROM AdresKontrahenta WHERE Ulica = 'TEST_Ulica_Beta')
    BEGIN
        INSERT INTO AdresKontrahenta (IdKontrahenta, Ulica, NrDomu, NrLokalu, KodPocztowy, Miasto, Kraj, CzyDomyslny)
        VALUES (@IdKontrahentaBeta, 'TEST_Ulica_Beta', '2', '2', '00-002', 'TEST_Krakow', 'TEST_Polska', 1);
    END;

    DECLARE @IdKategoriiElektronika INT = (
        SELECT TOP 1 IdKategoriiTowaru FROM KategoriaTowaru WHERE Nazwa = 'TEST_Elektronika'
    );
    DECLARE @IdKategoriiUslugi INT = (
        SELECT TOP 1 IdKategoriiTowaru FROM KategoriaTowaru WHERE Nazwa = 'TEST_Usługi'
    );
    DECLARE @IdJednostkiSzt INT = (
        SELECT TOP 1 IdJednostkiMiary FROM JednostkaMiary WHERE Symbol = 'TEST_szt'
    );
    DECLARE @IdJednostkiUsl INT = (
        SELECT TOP 1 IdJednostkiMiary FROM JednostkaMiary WHERE Symbol = 'TEST_usl'
    );
    DECLARE @IdVat23 INT = (
        SELECT TOP 1 IdStawkiVat FROM StawkaVat WHERE Nazwa = 'TEST_VAT23'
    );
    DECLARE @IdVat8 INT = (
        SELECT TOP 1 IdStawkiVat FROM StawkaVat WHERE Nazwa = 'TEST_VAT8'
    );

    IF NOT EXISTS (SELECT 1 FROM Towar WHERE Kod = 'TEST_TOWAR_1')
    BEGIN
        INSERT INTO Towar (
            Kod,
            Nazwa,
            IdKategoriiTowaru,
            IdJednostkiMiary,
            IdStawkiVatSprzedazy,
            IdStawkiVatZakupu,
            Cena,
            Marza,
            CzyAktywny
        )
        VALUES (
            'TEST_TOWAR_1',
            'TEST_Laptop',
            @IdKategoriiElektronika,
            @IdJednostkiSzt,
            @IdVat23,
            @IdVat23,
            4500.00,
            800.00,
            1
        );
    END;

    IF NOT EXISTS (SELECT 1 FROM Towar WHERE Kod = 'TEST_TOWAR_2')
    BEGIN
        INSERT INTO Towar (
            Kod,
            Nazwa,
            IdKategoriiTowaru,
            IdJednostkiMiary,
            IdStawkiVatSprzedazy,
            IdStawkiVatZakupu,
            Cena,
            Marza,
            CzyAktywny
        )
        VALUES (
            'TEST_TOWAR_2',
            'TEST_Serwis',
            @IdKategoriiUslugi,
            @IdJednostkiUsl,
            @IdVat8,
            @IdVat8,
            350.00,
            120.00,
            1
        );
    END;

    DECLARE @IdTowar1 INT = (
        SELECT TOP 1 IdTowaru FROM Towar WHERE Kod = 'TEST_TOWAR_1'
    );
    DECLARE @IdTowar2 INT = (
        SELECT TOP 1 IdTowaru FROM Towar WHERE Kod = 'TEST_TOWAR_2'
    );

    DECLARE @IdSposobPrzelew INT = (
        SELECT TOP 1 IdSposobuPlatnosci FROM SposobPlatnosci WHERE Nazwa = 'TEST_Przelew'
    );
    DECLARE @IdSposobGotowka INT = (
        SELECT TOP 1 IdSposobuPlatnosci FROM SposobPlatnosci WHERE Nazwa = 'TEST_Gotowka'
    );
    DECLARE @IdStatusWystawiona INT = (
        SELECT TOP 1 IdStatusuFaktury FROM StatusFaktury WHERE Nazwa = 'TEST_Wystawiona'
    );
    DECLARE @IdStatusOplacona INT = (
        SELECT TOP 1 IdStatusuFaktury FROM StatusFaktury WHERE Nazwa = 'TEST_Oplacona'
    );
    DECLARE @IdWalutaPln INT = (
        SELECT TOP 1 IdWaluty FROM Waluta WHERE Kod = 'TEST_PLN'
    );
    DECLARE @IdWalutaEur INT = (
        SELECT TOP 1 IdWaluty FROM Waluta WHERE Kod = 'TEST_EUR'
    );

    IF NOT EXISTS (SELECT 1 FROM FakturaSprzedazy WHERE Numer = 'TEST_FV_001')
    BEGIN
        INSERT INTO FakturaSprzedazy (
            Numer,
            DataWystawienia,
            DataSprzedazy,
            IdKontrahenta,
            IdSposobuPlatnosci,
            IdStatusuFaktury,
            IdWaluty,
            TerminPlatnosci,
            RazemNetto,
            RazemVat,
            RazemBrutto
        )
        VALUES (
            'TEST_FV_001',
            '2024-01-05',
            '2024-01-05',
            @IdKontrahentaAlfa,
            @IdSposobPrzelew,
            @IdStatusWystawiona,
            @IdWalutaPln,
            '2024-01-12',
            9000.00,
            2070.00,
            11070.00
        );
    END;

    IF NOT EXISTS (SELECT 1 FROM FakturaSprzedazy WHERE Numer = 'TEST_FV_002')
    BEGIN
        INSERT INTO FakturaSprzedazy (
            Numer,
            DataWystawienia,
            DataSprzedazy,
            IdKontrahenta,
            IdSposobuPlatnosci,
            IdStatusuFaktury,
            IdWaluty,
            TerminPlatnosci,
            RazemNetto,
            RazemVat,
            RazemBrutto
        )
        VALUES (
            'TEST_FV_002',
            '2024-02-10',
            '2024-02-10',
            @IdKontrahentaBeta,
            @IdSposobGotowka,
            @IdStatusOplacona,
            @IdWalutaEur,
            '2024-02-10',
            700.00,
            56.00,
            756.00
        );
    END;

    DECLARE @IdFaktura1 INT = (
        SELECT TOP 1 IdFakturySprzedazy FROM FakturaSprzedazy WHERE Numer = 'TEST_FV_001'
    );
    DECLARE @IdFaktura2 INT = (
        SELECT TOP 1 IdFakturySprzedazy FROM FakturaSprzedazy WHERE Numer = 'TEST_FV_002'
    );

    IF NOT EXISTS (SELECT 1 FROM PozycjaFakturySprzedazy WHERE IdFakturySprzedazy = @IdFaktura1 AND IdTowaru = @IdTowar1)
    BEGIN
        INSERT INTO PozycjaFakturySprzedazy (
            IdFakturySprzedazy,
            IdTowaru,
            Ilosc,
            CenaNetto,
            IdStawkiVat,
            WartoscNetto,
            WartoscVat,
            WartoscBrutto
        )
        VALUES (
            @IdFaktura1,
            @IdTowar1,
            2.000,
            4500.00,
            @IdVat23,
            9000.00,
            2070.00,
            11070.00
        );
    END;

    IF NOT EXISTS (SELECT 1 FROM PozycjaFakturySprzedazy WHERE IdFakturySprzedazy = @IdFaktura2 AND IdTowaru = @IdTowar2)
    BEGIN
        INSERT INTO PozycjaFakturySprzedazy (
            IdFakturySprzedazy,
            IdTowaru,
            Ilosc,
            CenaNetto,
            IdStawkiVat,
            WartoscNetto,
            WartoscVat,
            WartoscBrutto
        )
        VALUES (
            @IdFaktura2,
            @IdTowar2,
            2.000,
            350.00,
            @IdVat8,
            700.00,
            56.00,
            756.00
        );
    END;

    DECLARE @IdStatusZamowieniaNowe INT = (
        SELECT TOP 1 IdStatusuZamowienia FROM StatusZamowienia WHERE Nazwa = 'TEST_Nowe'
    );
    DECLARE @IdStatusZamowieniaRealizacja INT = (
        SELECT TOP 1 IdStatusuZamowienia FROM StatusZamowienia WHERE Nazwa = 'TEST_WRealizacji'
    );

    IF NOT EXISTS (SELECT 1 FROM ZamowienieSprzedazy WHERE Numer = 'TEST_ZS_001')
    BEGIN
        INSERT INTO ZamowienieSprzedazy (Numer, DataZamowienia, IdKontrahenta, IdStatusuZamowienia, Uwagi)
        VALUES ('TEST_ZS_001', '2024-03-01', @IdKontrahentaAlfa, @IdStatusZamowieniaNowe, 'Testowe zamówienie 1');
    END;

    IF NOT EXISTS (SELECT 1 FROM ZamowienieSprzedazy WHERE Numer = 'TEST_ZS_002')
    BEGIN
        INSERT INTO ZamowienieSprzedazy (Numer, DataZamowienia, IdKontrahenta, IdStatusuZamowienia, Uwagi)
        VALUES ('TEST_ZS_002', '2024-03-05', @IdKontrahentaBeta, @IdStatusZamowieniaRealizacja, 'Testowe zamówienie 2');
    END;

    DECLARE @IdZamowienie1 INT = (
        SELECT TOP 1 IdZamowieniaSprzedazy FROM ZamowienieSprzedazy WHERE Numer = 'TEST_ZS_001'
    );
    DECLARE @IdZamowienie2 INT = (
        SELECT TOP 1 IdZamowieniaSprzedazy FROM ZamowienieSprzedazy WHERE Numer = 'TEST_ZS_002'
    );

    IF NOT EXISTS (SELECT 1 FROM PozycjaZamowieniaSprzedazy WHERE IdZamowieniaSprzedazy = @IdZamowienie1 AND IdTowaru = @IdTowar1)
    BEGIN
        INSERT INTO PozycjaZamowieniaSprzedazy (IdZamowieniaSprzedazy, IdTowaru, Ilosc, CenaNetto)
        VALUES (@IdZamowienie1, @IdTowar1, 1.000, 4500.00);
    END;

    IF NOT EXISTS (SELECT 1 FROM PozycjaZamowieniaSprzedazy WHERE IdZamowieniaSprzedazy = @IdZamowienie2 AND IdTowaru = @IdTowar2)
    BEGIN
        INSERT INTO PozycjaZamowieniaSprzedazy (IdZamowieniaSprzedazy, IdTowaru, Ilosc, CenaNetto)
        VALUES (@IdZamowienie2, @IdTowar2, 3.000, 350.00);
    END;

    DECLARE @IdMagazyn1 INT = (
        SELECT TOP 1 IdMagazynu FROM Magazyn WHERE Kod = 'TEST_MAG1'
    );
    DECLARE @IdMagazyn2 INT = (
        SELECT TOP 1 IdMagazynu FROM Magazyn WHERE Kod = 'TEST_MAG2'
    );
    DECLARE @IdTypDokPz INT = (
        SELECT TOP 1 IdTypuDokumentu FROM TypDokumentuMagazynowego WHERE Kod = 'TEST_PZ'
    );
    DECLARE @IdTypDokWz INT = (
        SELECT TOP 1 IdTypuDokumentu FROM TypDokumentuMagazynowego WHERE Kod = 'TEST_WZ'
    );

    IF NOT EXISTS (SELECT 1 FROM DokumentMagazynowy WHERE Numer = 'TEST_DM_001')
    BEGIN
        INSERT INTO DokumentMagazynowy (Numer, DataDokumentu, IdMagazynu, IdTypuDokumentu, Uwagi)
        VALUES ('TEST_DM_001', '2024-04-01', @IdMagazyn1, @IdTypDokPz, 'Testowy dokument 1');
    END;

    IF NOT EXISTS (SELECT 1 FROM DokumentMagazynowy WHERE Numer = 'TEST_DM_002')
    BEGIN
        INSERT INTO DokumentMagazynowy (Numer, DataDokumentu, IdMagazynu, IdTypuDokumentu, Uwagi)
        VALUES ('TEST_DM_002', '2024-04-02', @IdMagazyn2, @IdTypDokWz, 'Testowy dokument 2');
    END;

    DECLARE @IdDokument1 INT = (
        SELECT TOP 1 IdDokumentuMagazynowego FROM DokumentMagazynowy WHERE Numer = 'TEST_DM_001'
    );
    DECLARE @IdDokument2 INT = (
        SELECT TOP 1 IdDokumentuMagazynowego FROM DokumentMagazynowy WHERE Numer = 'TEST_DM_002'
    );

    IF NOT EXISTS (SELECT 1 FROM PozycjaDokumentuMagazynowego WHERE IdDokumentuMagazynowego = @IdDokument1 AND IdTowaru = @IdTowar1)
    BEGIN
        INSERT INTO PozycjaDokumentuMagazynowego (IdDokumentuMagazynowego, IdTowaru, Ilosc)
        VALUES (@IdDokument1, @IdTowar1, 5.000);
    END;

    IF NOT EXISTS (SELECT 1 FROM PozycjaDokumentuMagazynowego WHERE IdDokumentuMagazynowego = @IdDokument2 AND IdTowaru = @IdTowar2)
    BEGIN
        INSERT INTO PozycjaDokumentuMagazynowego (IdDokumentuMagazynowego, IdTowaru, Ilosc)
        VALUES (@IdDokument2, @IdTowar2, 10.000);
    END;

    IF NOT EXISTS (SELECT 1 FROM StanMagazynowy WHERE IdMagazynu = @IdMagazyn1 AND IdTowaru = @IdTowar1)
    BEGIN
        INSERT INTO StanMagazynowy (IdMagazynu, IdTowaru, Ilosc)
        VALUES (@IdMagazyn1, @IdTowar1, 5.000);
    END;

    IF NOT EXISTS (SELECT 1 FROM StanMagazynowy WHERE IdMagazynu = @IdMagazyn2 AND IdTowaru = @IdTowar2)
    BEGIN
        INSERT INTO StanMagazynowy (IdMagazynu, IdTowaru, Ilosc)
        VALUES (@IdMagazyn2, @IdTowar2, 10.000);
    END;

    COMMIT TRANSACTION;

    SELECT 'SposobPlatnosci' AS Tabela, COUNT(*) AS Liczba FROM SposobPlatnosci
    UNION ALL SELECT 'StatusFaktury', COUNT(*) FROM StatusFaktury
    UNION ALL SELECT 'KategoriaTowaru', COUNT(*) FROM KategoriaTowaru
    UNION ALL SELECT 'JednostkaMiary', COUNT(*) FROM JednostkaMiary
    UNION ALL SELECT 'Waluta', COUNT(*) FROM Waluta
    UNION ALL SELECT 'StawkaVat', COUNT(*) FROM StawkaVat
    UNION ALL SELECT 'TypKontrahenta', COUNT(*) FROM TypKontrahenta
    UNION ALL SELECT 'StatusZamowienia', COUNT(*) FROM StatusZamowienia
    UNION ALL SELECT 'Magazyn', COUNT(*) FROM Magazyn
    UNION ALL SELECT 'TypDokumentuMagazynowego', COUNT(*) FROM TypDokumentuMagazynowego
    UNION ALL SELECT 'Kontrahent', COUNT(*) FROM Kontrahent
    UNION ALL SELECT 'AdresKontrahenta', COUNT(*) FROM AdresKontrahenta
    UNION ALL SELECT 'Towar', COUNT(*) FROM Towar
    UNION ALL SELECT 'FakturaSprzedazy', COUNT(*) FROM FakturaSprzedazy
    UNION ALL SELECT 'PozycjaFakturySprzedazy', COUNT(*) FROM PozycjaFakturySprzedazy
    UNION ALL SELECT 'ZamowienieSprzedazy', COUNT(*) FROM ZamowienieSprzedazy
    UNION ALL SELECT 'PozycjaZamowieniaSprzedazy', COUNT(*) FROM PozycjaZamowieniaSprzedazy
    UNION ALL SELECT 'DokumentMagazynowy', COUNT(*) FROM DokumentMagazynowy
    UNION ALL SELECT 'PozycjaDokumentuMagazynowego', COUNT(*) FROM PozycjaDokumentuMagazynowego
    UNION ALL SELECT 'StanMagazynowy', COUNT(*) FROM StanMagazynowy;

    SELECT
        f.Numer,
        k.NazwaPelna AS Kontrahent,
        s.Nazwa AS Status,
        w.Kod AS Waluta,
        f.RazemNetto,
        f.RazemVat,
        f.RazemBrutto
    FROM FakturaSprzedazy f
    INNER JOIN Kontrahent k ON k.IdKontrahenta = f.IdKontrahenta
    INNER JOIN StatusFaktury s ON s.IdStatusuFaktury = f.IdStatusuFaktury
    INNER JOIN Waluta w ON w.IdWaluty = f.IdWaluty
    WHERE f.Numer LIKE 'TEST_%';

    SELECT
        z.Numer,
        k.NazwaPelna AS Kontrahent,
        sz.Nazwa AS Status,
        z.DataZamowienia
    FROM ZamowienieSprzedazy z
    INNER JOIN Kontrahent k ON k.IdKontrahenta = z.IdKontrahenta
    INNER JOIN StatusZamowienia sz ON sz.IdStatusuZamowienia = z.IdStatusuZamowienia
    WHERE z.Numer LIKE 'TEST_%';

    SELECT
        d.Numer,
        m.Nazwa AS Magazyn,
        t.Nazwa AS TypDokumentu,
        d.DataDokumentu
    FROM DokumentMagazynowy d
    INNER JOIN Magazyn m ON m.IdMagazynu = d.IdMagazynu
    INNER JOIN TypDokumentuMagazynowego t ON t.IdTypuDokumentu = d.IdTypuDokumentu
    WHERE d.Numer LIKE 'TEST_%';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;

    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;
