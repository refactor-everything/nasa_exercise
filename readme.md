# NASA Programming Exercise

This is a programming exercise that utilizes the NASA API to download images (https://api.nasa.gov/).

# Requirements

* Visual Studio 2022
* Git
* NASA API Key

# Build Instructions

* Add an "ApiKey" property in your `secrets.json` file (Solution Explorer > NasaService > right-click > Manage User Secrets), and input the API key you generated after creating your NASA account.
* Build and run the solution.
* Images will download to the `image_download` folder in the working directory.
* If you want to ingest a pre-downloaded NASA JSON file containing Mars Rover image metadata (i.e. instead of using the API, particularly during testing), supply a `FilePath` value under the `NasaFileOptions` node. If this value is blank, the app will use the API.