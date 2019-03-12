(function () {
    'use strict';

    function UmbracoOCR($scope, $http, editorState, navigationService) {

        var vm = this;
        var apiUrl;

        function init() {
            apiUrl = Umbraco.Sys.ServerVariables["UmbracoOCR"]["UmbracoOCRApiUrl"];

            $scope.textFromImage = '';

        }

        $scope.imageChanged = function () {

            var file = document.getElementById("image").files[0];
            var dataUri = URL.createObjectURL(file);

            alert(dataUri);

            $http.get(apiUrl + 'GetTextFromImageAsync').then(function (response) {
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
                    text += '\n';
                }
                $scope.textFromImage = text.trim();
            });
        }

        init();

    }

    angular.module('umbraco').controller('UmbracoOCR', UmbracoOCR);

})();