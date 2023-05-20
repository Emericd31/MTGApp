# MTGApp
Application to manage your Magic Collection

Prerequisites : 
    - You must have .Net 6.0 installed on your machine
        - It can be downloaded at the following address : "https://dotnet.microsoft.com/en-us/download"

To make this application work, follow these steps : 
    - Download the contents of the "Release" folder
    - Edit the "BlazorApp.staticwebassets.runtime.json" file : 
        - Line 3 : Indicate the absolute path to your "wwwroot" folder
        - Line 4 : Indicate the absolute path to the  current folder
    - Launch "BlazorApp.exe" (A console should open)
    - Go to "http://localhost:5001/" or "https://localhost:5001/"

Important : The data related to your collection or decks are stored in .json files located in the "UserData" folder.
Do not delete this folder and its contents
This folder and its contents must be kept even if you update the application
