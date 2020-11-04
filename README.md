# doctorCSharp

# Orvos - asszisztens
> Egy orvosi rendelőben működő kliens - szerver alkalmazás implementálása.

## Asszisztens kliens - .NET WPF asztali alkalmazás
> A asszisztens pultján működik.
- #### Az érkező betegeket tudja rögzíteni
    - Név
        - Validáció
        - UNIT teszt
    - Lakcím
    - Tajszám `Formátum: 000 000 000`
        - Validáció
        - UNIT teszt
    - Panasz rövid leírása

## Orvos kliens - .NET WPF asztali alkalmazás
> A orvos irodájában működik.
- #### Látja a felvett betegek listáját
    - Időrendi sorrendben rendezve
    - Ki tud választani egy beteget
        - Látja az adatait
        - Tudja módosítani
            - Diagnózis felvétele
        - Tudja törölni

## Szerver - .NET WEB API alkalmazás (önálló konzol alkalmazás)
- #### Tárolja és szolgáltatja a bevitt adatokat
    - Adatok tárolása: JSON, XML vagy adatbázis
    - Indításkor betölti a korábbi adatokat
