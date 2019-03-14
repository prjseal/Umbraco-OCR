(function () {
    'use strict';

    function UmbracoOCR($scope, $http, editorState, navigationService) {

        var vm = this;
        var apiUrl;

        function init() {
            apiUrl = Umbraco.Sys.ServerVariables["UmbracoOCR"]["UmbracoOCRApiUrl"];

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
                for (var r = 0; r < textData["regions"].length; r++) {
                    var region = textData["regions"][r];
                    for (var l = 0; l < region["lines"].length; l++) {
                        var line = region["lines"][l];
                        for (var w = 0; w < line["words"].length; w++) {
                            var word = line["words"][w];
                            text += word["text"] + ' ';
                        }
                        text += '\n';
                    }
                    text += '\n\n';
                }
                $scope.textFromImage = text.trim();
                }).error(function() {
                    $scope.textFromImage = 'There was an error when trying to get the text from the image.';
                });
        }

        init();

    }

    angular.module('umbraco').controller('UmbracoOCR', UmbracoOCR);

})();