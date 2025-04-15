# 3SemesterEksamen

## Draw.io
[Draw.io](https://app.diagrams.net/#Wb!2ZvumUSy5kaTxX9DdntarkwBS9uu9EFMqt0PTZeH00WpAHjXfAitRIPCBFyVKFFi%2F01U5PXUQMS2PQ4OTBHBVDKTKT6PF3Z7BAD#%7B%22pageId%22%3A%22qPxq1riveFJrDDAtsqm9%22%7D)

## Databaseopsætning og løbende opdatering
For at opsætte lokal database skal databasen laves på den lokale server og derefter skal følgende køres
1. I Package Manager Console, kør:
```shell
Add-Migration 'navn på migration'
```
2. Opdater Databasen
```shell
Update-Database
```
Dette skal køres hver gang databaseændringer skal opdateres til den lokale database. 

---
## Merging 
1. Lav pull Request på Mirror Branchen.
2. Merge ind på Mirror Branchen med det samme.
3. Vi merger Mirror Branchen med Main når vi sidder sammen.
