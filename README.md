# Dialogix

## Project Overview

**Dialogix** is a real-time chat platform developed using .NET Core, designed to facilitate seamless communication between users. Leveraging SignalR for real-time messaging, Dialogix provides an engaging chat experience. The application integrates with SQL Server for reliable data storage and Amazon S3 for efficient image sharing. Users can effortlessly engage in conversations and share photos, enhancing their overall communication experience.

### Purpose

In today’s fast-paced world, effective communication is crucial. Dialogix aims to create a user-friendly platform where individuals can connect, share, and communicate without barriers. Whether for personal conversations or professional collaborations, Dialogix serves as an essential tool for fostering dialogue.

### Key Features

- **Real-time Messaging**: Enjoy instant communication with SignalR, ensuring messages are delivered in real-time.
- **User Authentication**: Secure user login and registration, protecting user data and privacy.
- **Photo Sharing**: Share images effortlessly through Amazon S3 integration, allowing users to upload and retrieve images securely.
- **Responsive Design**: A mobile-friendly interface that adjusts to different screen sizes, ensuring a smooth user experience on all devices.
- **Chat History**: Access previous conversations for continuity and reference.
- **Notifications**: Stay updated with real-time notifications for incoming messages and alerts.
- **User Profiles**: Customize user profiles with images and bios, enhancing the community feel of the platform.

## Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [Directory Structure](#directory-structure)
- [Setup Instructions](#setup-instructions)
- [API Documentation](#api-documentation)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Deployment](#deployment)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)
- [Author](#author)

## Technology Stack

- **Backend**: .NET Core
- **Database**: SQL Server
- **Real-time Communication**: SignalR
- **Cloud Storage**: Amazon S3
- **Frontend**: HTML, CSS, JavaScript
- **Build Tools**: .NET CLI, Visual Studio
- **Testing Framework**: xUnit for unit testing

## Directory Structure

Here's a brief overview of the directory structure of the Dialogix project:

```
├── .gitattributes
├── .gitignore
├── API/
│   ├── ChatAPIController.cs
│   └── LoginAPIController.cs
├── AppCode/
│   └── SessionService.cs
├── appsettings.Development.json
├── appsettings.json
├── bin/ (ignored)
├── Controllers/
│   ├── ChatController.cs
│   ├── ConnectionsController.cs
│   ├── Firebase/
│   │   └── FirebaseController.cs
│   ├── HomeController.cs
│   └── LoginController.cs
├── DB Scripts/
│   └── Dialogix.sql
├── Dialogix.csproj
├── Dialogix.sln
├── Hubs/
│   └── ChatHub.cs
├── libman.json
├── LICENSE
├── Models/
│   ├── ChatMessage.cs
│   ├── ChatUser.cs
│   ├── ErrorViewModel.cs
│   ├── FirebaseConfig.cs
│   ├── SendNotificationMessage.cs
│   ├── UserConnection.cs
│   └── UserPendingNotification.cs
├── obj/ (ignored)
├── Program.cs
├── Properties/
│   └── launchSettings.json
├── README.md
├── Views/
│   ├── Chat/
│   │   ├── ChatBox.cshtml
│   │   └── ChatMessage.cshtml
│   ├── Connections/
│   │   └── ConnectionList.cshtml
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   ├── Login/
│   │   ├── Login.cshtml
│   │   ├── Register.cshtml
│   │   └── _partialTermsCondition.cshtml
│   ├── Shared/
│   │   ├── Error.cshtml
│   │   ├── _Layout.cshtml
│   │   ├── _Layout.cshtml.css
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── web.config
└── wwwroot/
    ├── css/
    │   ├── bootstrap.min.css
    │   └── site.css
    ├── favicon.ico
    ├── firebase-messaging-sw.js
    ├── images/
    │   ├── camera-svgrepo-com.svg
    │   ├── camera2.svg
    │   ├── camera3.svg
    │   ├── loginChat.svg
    │   ├── registerChat.svg
    │   ├── svg1.svg
    │   ├── svg2.svg
    │   ├── svg3.svg
    │   └── termsConditon.svg
    ├── js/
    │   ├── firebase-app-compat.js
    │   ├── firebase-app.js
    │   ├── firebase-messaging-compat.js
    │   ├── firebase-messaging.js
    │   ├── signalr.js
    │   ├── site.js
    │   ├── toastr.min.css
    │   ├── toastr.min.js
    │   └── webcam.min.js
    └── lib/
        ├── bootstrap/
        │   ├── dist/
        │   │   ├── css/
        │   │   │   ├── bootstrap-grid.css
        │   │   │   ├── bootstrap-grid.css.map
        │   │   │   ├── bootstrap-grid.min.css
        │   │   │   ├── bootstrap-grid.min.css.map
        │   │   │   ├── bootstrap-grid.rtl.css
        │   │   │   ├── bootstrap-grid.rtl.css.map
        │   │   │   ├── bootstrap-grid.rtl.min.css
        │   │   │   ├── bootstrap-grid.rtl.min.css.map
        │   │   │   ├── bootstrap-reboot.css
        │   │   │   ├── bootstrap-reboot.css.map
        │   │   │   ├── bootstrap-reboot.min.css
        │   │   │   ├── bootstrap-reboot.min.css.map
        │   │   │   ├── bootstrap-reboot.rtl.css
        │   │   │   ├── bootstrap-reboot.rtl.css.map
        │   │   │   ├── bootstrap-reboot.rtl.min.css
        │   │   │   ├── bootstrap-reboot.rtl.min.css.map
        │   │   │   ├── bootstrap-utilities.css
        │   │   │   ├── bootstrap-utilities.css.map
        │   │   │   ├── bootstrap-utilities.min.css
        │   │   │   ├── bootstrap-utilities.min.css.map
        │   │   │   ├── bootstrap-utilities.rtl.css
        │   │   │   ├── bootstrap-utilities.rtl.css.map
        │   │   │   ├── bootstrap-utilities.rtl.min.css
        │   │   │   ├── bootstrap-utilities.rtl.min.css.map
        │   │   │   ├── bootstrap.css
        │   │   │   ├── bootstrap.css.map
        │   │   │   ├── bootstrap.min.css
        │   │   │   ├── bootstrap.min.css.map
        │   │   │   ├── bootstrap.rtl.css
        │   │   │   ├── bootstrap.rtl.css.map
        │   │   │   ├── bootstrap.rtl.min.css
        │   │   │   └── bootstrap.rtl.min.css.map
        │   │   └── js/
        │   │       ├── bootstrap.bundle.js
        │   │       ├── bootstrap.bundle.js.map
        │   │       ├── bootstrap.bundle.min.js
        │   │       ├── bootstrap.bundle.min.js.map
        │   │       ├── bootstrap.esm.js
        │   │       ├── bootstrap.esm.js.map
        │   │       ├── bootstrap.esm.min.js
        │   │       ├── bootstrap.esm.min.js.map
        │   │       ├── bootstrap.js
        │   │       ├── bootstrap.js.map
        │   │       ├── bootstrap.min.js
        │   │       └── bootstrap.min.js.map
        │   └── LICENSE
        ├── jquery/
        │   ├── dist/
        │   │   ├── jquery.js
        │   │   ├── jquery.min.js
        │  

 │   ├── jquery.min.map
        │   │   └── jquery.slim.js
        │   └── LICENSE
        └── popper.js/
            ├── dist/
            │   ├── popper.js
            │   └── popper.min.js
            └── LICENSE
```

## Setup Instructions

To set up Dialogix locally, follow these steps:

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/Pirate-Emperor/Dialogix.git
   cd Dialogix
   ```

2. **Install Required Packages**:
   Use the .NET CLI to restore the necessary packages.
   ```bash
   dotnet restore
   ```

3. **Configure Database**:
   - Create a new SQL Server database and run the database scripts located in the `DB Scripts` folder to set up the required tables and schema.

4. **Configure AWS S3**:
   - Set up an Amazon S3 bucket to handle image uploads and updates.
   - Add your AWS credentials in `appsettings.json` under the `AWS` section.

5. **Build and Run the Application**:
   ```bash
   dotnet build
   dotnet run
   ```

6. **Access the Application**:
   Open your web browser and navigate to `http://localhost:5000` to start using Dialogix.

## API Documentation

### Chat API

- **Retrieve Chat Messages**:
  - `GET /api/chat/messages`
    - Description: Retrieves a list of chat messages for the current user.
    - Response: Returns a JSON array of chat messages.

- **Send a Chat Message**:
  - `POST /api/chat/send`
    - Description: Sends a new chat message.
    - Request Body: JSON object containing the message text and recipient ID.
    - Response: Confirmation of message sent.

### Login API

- **Authenticate User**:
  - `POST /api/login`
    - Description: Authenticates a user based on credentials.
    - Request Body: JSON object containing username and password.
    - Response: User authentication token if successful.

## Running the Application

To run the application locally:

1. Ensure your SQL Server and Amazon S3 configurations are correctly set.
2. Open a terminal and navigate to the project directory.
3. Execute the following command:
   ```bash
   dotnet run
   ```

4. Open a web browser and visit `http://localhost:5000` to interact with the application.

### Using Dialogix

- **Register a New User**: Click on the "Register" link and fill in your details to create a new account.
- **Log In**: Use your credentials to log in and access the chat functionalities.
- **Start Chatting**: Once logged in, you can view ongoing conversations, send messages, and share images with other users.

## Testing

To run unit tests for the application, navigate to the project directory and use the following command:

```bash
dotnet test
```

This command will execute all the test cases defined in the project, ensuring that your application functions correctly.

## Deployment

### Deploying to Azure

To deploy Dialogix to Azure:

1. Create a new Azure App Service.
2. Configure your Azure SQL Database and link it to your App Service.
3. Publish your application using Visual Studio or the Azure CLI.
4. Ensure that your Azure App Service settings mirror your local `appsettings.json` configurations for AWS and SQL Server.

## Troubleshooting

- **Common Issues**:
  - If you encounter issues connecting to SQL Server, ensure that the server is running and that the connection strings in `appsettings.json` are correct.
  - For Amazon S3 issues, verify your bucket permissions and ensure that your AWS credentials are correctly set.

- **Error Logs**:
  Check the logs generated by the application for any errors. Logs are typically found in the `logs` directory or outputted to the console during runtime.

## Contributing

Feel free to fork the repository, make changes, and submit pull requests. Contributions are welcome!

## License

This project is licensed under the Pirate-Emperor License. See the [LICENSE](LICENSE) file for details.

## Author

**Pirate-Emperor**

[![Twitter](https://skillicons.dev/icons?i=twitter)](https://twitter.com/PirateKingRahul)
[![Discord](https://skillicons.dev/icons?i=discord)](https://discord.com/users/1200728704981143634)
[![LinkedIn](https://skillicons.dev/icons?i=linkedin)](https://www.linkedin.com/in/piratekingrahul)

[![Reddit](https://img.shields.io/badge/Reddit-FF5700?style=for-the-badge&logo=reddit&logoColor=white)](https://www.reddit.com/u/PirateKingRahul)
[![Medium](https://img.shields.io/badge/Medium-42404E?style=for-the-badge&logo=medium&logoColor=white)](https://medium.com/@piratekingrahul)

- GitHub: [Pirate-Emperor](https://github.com/Pirate-Emperor)
- Reddit: [PirateKingRahul](https://www.reddit.com/u/PirateKingRahul/)
- Twitter: [PirateKingRahul](https://twitter.com/PirateKingRahul)
- Discord: [PirateKingRahul](https://discord.com/users/1200728704981143634)
- LinkedIn: [PirateKingRahul](https://www.linkedin.com/in/piratekingrahul)
- Skype: [Join Skype](https://join.skype.com/invite/yfjOJG3wv9Ki)
- Medium: [PirateKingRahul](https://medium.com/@piratekingrahul)

---
