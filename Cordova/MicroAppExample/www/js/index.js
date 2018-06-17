/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
var app = {
    // Application Constructor
    initialize: function () {
        document.addEventListener('deviceready', this.onDeviceReady.bind(this), false);
    },

    // deviceready Event Handler
    //
    // Bind any cordova events here. Common events are:
    // 'pause', 'resume', etc.
    onDeviceReady: function () {
        this.receivedEvent('deviceready');

        this.customGetFileExample();

        this.writeToFile('example.json', { foo: 'bar' });
    },

    // Update DOM on a Received Event
    receivedEvent: function (id) {
        var parentElement = document.getElementById(id);
        var listeningElement = parentElement.querySelector('.listening');
        var receivedElement = parentElement.querySelector('.received');

        listeningElement.setAttribute('style', 'display:none;');
        receivedElement.setAttribute('style', 'display:block;');

        console.log('Received Event: ' + id);
    },

    customGetFileExample: function () {
        var customGetFileList = function () {

            var url = 'http://10.0.2.2/API/ClientApp/';
            url = 'http://localhost:5000/Api/CordovaApp/';
            var oReq = new XMLHttpRequest();
            // Make sure you add the domain name to the Content-Security-Policy <meta> element.
            //oReq.open("GET", "http://cordova.apache.org/static/img/cordova_bot.png", true);

            oReq.open("GET", url);
            oReq.onload = function (oEvent) {

                console.log(oReq.response);
                console.log(oReq.responseText);
            };
            oReq.send(null);
        }


        var url = 'http://10.0.2.2:5000/API/ClientApp/';


        customGetFileList();

        return;


        window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
            console.log('file system open: ' + fs.name);
            fs.root.getFile('bot.png', { create: true, exclusive: false }, function (fileEntry) {
                console.log('fileEntry is file? ' + fileEntry.isFile.toString());
                var oReq = new XMLHttpRequest();
                // Make sure you add the domain name to the Content-Security-Policy <meta> element.
                oReq.open("GET", "http://cordova.apache.org/static/img/cordova_bot.png", true);
                // Define how you want the XHR data to come back
                oReq.responseType = "blob";
                oReq.onload = function (oEvent) {
                    var blob = oReq.response; // Note: not oReq.responseText
                    if (blob) {
                        // Create a URL based on the blob, and set an <img> tag's src to it.
                        var url = window.URL.createObjectURL(blob);
                        document.getElementById('bot-img').src = url;
                        // Or read the data with a FileReader
                        var reader = new FileReader();
                        reader.addEventListener("loadend", function () {
                            // reader.result contains the contents of blob as text
                        });
                        reader.readAsText(blob);
                    } else console.error('we didnt get an XHR response!');
                };
                oReq.send(null);
            }, function (err) { console.error('error getting file! ' + err); });
        }, function (err) { console.error('error getting persistent fs! ' + err); });

    },
    writeToFile: function (fileName, data) {

        window.requestFileSystem  = window.requestFileSystem || window.webkitRequestFileSystem;

        var errorHandler = function (fileName, e) {
            var msg = '';

            switch (e.code) {
                case FileError.QUOTA_EXCEEDED_ERR:
                    msg = 'Storage quota exceeded';
                    break;
                case FileError.NOT_FOUND_ERR:
                    msg = 'File not found';
                    break;
                case FileError.SECURITY_ERR:
                    msg = 'Security error';
                    break;
                case FileError.INVALID_MODIFICATION_ERR:
                    msg = 'Invalid modification';
                    break;
                case FileError.INVALID_STATE_ERR:
                    msg = 'Invalid state';
                    break;
                default:
                    msg = 'Unknown error';
                    break;
            };

            console.log('Error (' + fileName + '): ' + msg);
        }

        data = JSON.stringify(data, null, '\t');

        function writeFile(fs) {

            fs.root.getFile('log.txt', {create: true}, function(fileEntry) {
          
              // Create a FileWriter object for our FileEntry (log.txt).
              fileEntry.createWriter(function(fileWriter) {
          
                fileWriter.onwriteend = function(e) {
                  console.log('Write completed.');
                };
          
                fileWriter.onerror = function(e) {
                  console.log('Write failed: ' + e.toString());
                };
          
                // Create a new Blob and write it to log.txt.
                var blob = new Blob(['Lorem Ipsum'], {type: 'text/plain'});
          
                fileWriter.write(blob);
          
              }, errorHandler);
          
            }, errorHandler);
          
          }
          function onInitFs(fs) {

            fs.root.getFile('log.txt', {}, function(fileEntry) {
          
              // Get a File object representing the file,
              // then use FileReader to read its contents.
              fileEntry.file(function(file) {
                 var reader = new FileReader();
          
                 reader.onloadend = function(e) {
                   var txtArea = document.createElement('textarea');
                   txtArea.value = this.result;
                   console.log(this.result);
                   
                   document.body.appendChild(txtArea);
                 };
          
                 reader.readAsText(file);
              }, errorHandler);
          
            }, errorHandler);
          
          }
          
          
          
          window.requestFileSystem(window.PERSISTENT, 1024*1024, writeFile, errorHandler);
          window.requestFileSystem(window.PERSISTENT, 1024*1024, onInitFs, errorHandler);

        // window.resolveLocalFileSystemURL(cordova.file.dataDirectory, function (directoryEntry) {
        //     directoryEntry.getFile(fileName, { create: true }, function (fileEntry) {
        //         fileEntry.createWriter(function (fileWriter) {
        //             fileWriter.onwriteend = function (e) {
        //                 // for real-world usage, you might consider passing a success callback
        //                 console.log('Write of file "' + fileName + '"" completed.');
        //             };

        //             fileWriter.onerror = function (e) {
        //                 // you could hook this up with our global error handler, or pass in an error callback
        //                 console.log('Write failed: ' + e.toString());
        //             };

        //             var blob = new Blob([data], { type: 'text/plain' });
        //             fileWriter.write(blob);
        //         }, errorHandler.bind(null, fileName));
        //     }, errorHandler.bind(null, fileName));
        // }, errorHandler.bind(null, fileName));
    }



};

app.initialize();