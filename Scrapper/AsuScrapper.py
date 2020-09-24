import re
import requests as req
from bs4 import BeautifulSoup as bs

baseUrl = "https://tours.asu.edu"
campusUrl = "/tempe/buildings"
buildings = []


def getData(filename, buildings):
    buildingsData = []
    for url in buildings:
        buildingPage = req.get(baseUrl+url)
        soup = bs(buildingPage.content, 'lxml')
        name = soup.find_all("h1")[0].string
        location = soup.find("div", class_="field-name-field-bldg-address")
        code = soup.find(
            "div", class_="field-name-field-bldg-code").contents[1].div.string
        try:
            gmapsLink = location.contents[1].div.contents[0].a['href']
            coordinates = re.findall('[34]d[-]?\d{0,3}\.\d{,7}', gmapsLink)
        except:
            coordinates = ["0000.0000", "0000.0000"]
            print(name, code)
        if len(coordinates) > 1:
            buildingObj = {}
            buildingObj['BuildingName'] = name
            buildingObj['BuildingCode'] = code
            buildingObj['Coordinates'] = {
                'Latitude': coordinates[0][3:], 'Longitude': coordinates[1][3:]}
            buildingsData.append(buildingObj)
    return buildingsData


basePage = req.get(baseUrl+campusUrl)
soup = bs(basePage.content, 'lxml')

for builingLink in soup.find_all("div", class_="field-content"):
    for i in builingLink:
        if i.name == "a":
            buildings.append(i['href'])

buildingsData = getData("Building_Data.csv", buildings)
print(buildingsData)
