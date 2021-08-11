# GIV Thurgau - Interlis2 Tool
Konsolen-Applikation für die Interlis2-Datendateien zu ändern.

# Funktionen
## DSS zu TG-Modelle
Konvertiert eine Interlis2-Datendatei im Modell DSS_2020_LV95 in das gewünschte Modell des Kanton Thurgau.
Es werden lediglich die XML-Tags in der XTF-Datei umgeschrieben, der Dateninhalt bleibt bestehen.
### convertDSS2TGMEL
Input: DSS_2020_LV95  
Output: DSS_2020_LV95_MEL  

Beispielaufruf in der Konsole:
```
"GIV Interlis2 Tools.exe" --type convertDSS2TGMEL --input "C:\Temp\DSS_2020.xtf" --output "C:\Temp\TG_2020_MEL.xtf" --log "C:\Temp\dss2tgmel.log"
```

### convertDSS2TGGEP
Input: DSS_2020_LV95  
Output: DSS_2020_LV95_GEP  

Beispielaufruf in der Konsole:
```
"GIV Interlis2 Tools.exe" --type convertDSS2TGGEP --input "C:\Temp\DSS_2020.xtf" --output "C:\Temp\TG_2020_GEP.xtf" --log "C:\Temp\dss2tggep.log"
```

## Datenfilter
Erstellt eine neue Datei mit den Daten, die dem vordefinierten Filter entsprechen.

### splitDSS2TGMEL
Input: DSS_2020_LV95  
Output: DSS_2020_LV95  
Filter: Kanal.FunktionMelioration und Abwasserknoten.Funktion_Knoten_Melioration  

Beispielaufruf in der Konsole:
```
"GIV Interlis2 Tools.exe" --type splitDSS2Melio --input "C:\Temp\DSS_2020.xtf" --output "C:\Temp\DSS_2020_MEL.xtf" --log "C:\Temp\tgmelfilter.log"
```