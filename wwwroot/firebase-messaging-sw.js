// Import Firebase libraries
importScripts('/js/firebase-app-compat.js');
importScripts('/js/firebase-messaging-compat.js');
debugger;
// Initialize Firebase
firebase.initializeApp({
    apiKey: "AIzaSyBIWES3c3n3-JXMAwSfp1bNZQ6oZ3yQGyc",
    authDomain: "realtimechatapplication-pirate-emperor.firebaseapp.com",
    projectId: "realtimechatapplication-pirate-emperor",
    storageBucket: "realtimechatapplication-pirate-emperor.appspot.com",
    messagingSenderId: "965800671505",
    appId: "1:965800671505:web:3f3238b4bf441f8708f4b0",
    measurementId: "G-LCJX2K7LR6"
});

// Retrieve an instance of Firebase Messaging
const messaging = firebase.messaging();

// Handle background messages
messaging.onBackgroundMessage(function (payload) {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);

    const notificationTitle = payload.notification.title || 'Background Message Title';
    const notificationOptions = {
        body: payload.notification.body,
        icon: '/firebase-logo.png' // Change this to your notification icon path
    };

    return self.registration.showNotification(notificationTitle, notificationOptions);
});