(function () {
    'use strict';

    function OcrContentApp($scope, $http) {
        var apiUrl;

        function init() {
            apiUrl = Umbraco.Sys.ServerVariables["OCR"]["OcrApiUrl"];

            $scope.textFromImage = '';
            $scope.imageUri = '';
        }

        $scope.imageChanged = function () {
            $scope.textFromImage = 'Please wait...';

            var preview = document.querySelector('#umbraco-ocr img');
            var file = document.getElementById("image").files[0];

            var reader = new FileReader();

            reader.addEventListener("load", function () {
                $scope.imageUri = reader.result;
                preview.src = $scope.imageUri;
                preview.style.display = "block";
                $scope.getTextFromImage();
            }, false);

            if (file) {
                reader.readAsDataURL(file);
            }
        }

        $scope.getTextFromImage = function () {
            $http({
                method: 'POST',
                url: apiUrl + 'GetTextFromImage/',
                data: JSON.stringify({ ImageUri: preview.src }),
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function (response) {
                var textData = JSON.parse(response.data);
                var text = '';
                if (!textData.error) {
                    if (textData != '') {
                        for (var r = 0; r < textData["regions"].length; r++) {
                            var region = textData["regions"][r];
                            for (var l = 0; l < region["lines"].length; l++) {
                                var line = region["lines"][l];
                                for (var w = 0; w < line["words"].length; w++) {
                                    var word = line["words"][w];
                                    text += word["text"] + ' ';
                                }
                                text = text.trim();
                                text += '\n';
                            }
                            text += '\n\n';
                        }
                    }
                } else {
                    console.log(textData.error.message);
                    text =
                        'There was an error. Check the console. ' +
                        '\n' +
                        '\nPlease make sure you have a valid subscription key for the Azure Vision API in the app settings of the web.config file.' +
                        '\nAlso check that the ocr api endpoint is correct' +
                        '\n' +
                        '\nYou need the following app settings in your web.config file:' +
                        '\n' +
                        '\n<add key="SubscriptionKey" value="azure-subscription-key-here" />' +
                        '\n<add key="ApiUrl" value="https://westeurope.api.cognitive.microsoft.com/vision/v2.0/ocr" />' +
                        '\n' +
                        '\nTo find out how to get a subscription key, visit this page:' +
                        '\nhttps://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/vision-api-how-to-topics/howtosubscribe';
                }
                $scope.textFromImage = text.trim();
            });
        }
        init();
    }

    angular.module('umbraco').controller('OcrContentApp', OcrContentApp);

})();