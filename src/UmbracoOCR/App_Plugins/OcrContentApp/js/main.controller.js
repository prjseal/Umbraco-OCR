(function () {
    'use strict';

    function OcrContentApp($scope, $http) {
        var apiUrl;

        function init() {
            apiUrl = Umbraco.Sys.ServerVariables["OCR"]["OcrApiUrl"];
            $scope.textFromImage = '';
            $scope.imageUri = '';
        }

        $scope.imageChanged = function ($event) {
            $scope.textFromImage = 'Please wait...';

            var preview = document.querySelector('#ocr-content-app img');
            var file = $event.target.files[0];

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
                data: JSON.stringify({ ImageUri: $scope.imageUri }),
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function (response) {
                var textData = JSON.parse(response.data);
                var text = '';
                if (!textData.error) {
                    if (textData != '') {
                        for (var r = 0; r < textData.regions.length; r++) {
                            var region = textData.regions[r];
                            for (var l = 0; l < region.lines.length; l++) {
                                var line = region.lines[l];
                                for (var w = 0; w < line.words.length; w++) {
                                    var word = line.words[w];
                                    text += word.text + ' ';
                                }
                                text = text.trim();
                                text += '\n';
                            }
                            text += '\n\n';
                        }
                    }
                } else {
                    text = textData.error.message;
                }
                $scope.textFromImage = text.trim();
            });
        }
        init();
    }

    angular.module('umbraco').controller('OcrContentApp', OcrContentApp);

    angular.module("umbraco").directive("ngUploadChange", function () {
        return {
            scope: {
                ngUploadChange: "&"
            },
            link: function ($scope, $element, $attrs) {
                $element.on("change",
                    function(event) {
                        $scope.$apply(function() {
                            $scope.ngUploadChange({ $event: event });
                        });
                    });
                $scope.$on("$destroy", function () {
                    $element.off();
                });
            }
        }
    });

})();