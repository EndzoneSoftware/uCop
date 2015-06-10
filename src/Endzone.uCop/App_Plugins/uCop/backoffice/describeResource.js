angular.module("umbraco.resources").factory("describeResource",
    function ($http) {
        return {
            getDocumentTypes: function () {
                return $http.get("backoffice/ucop/Describe/DocumentTypes");
            },

            getUrls: function (documentTypeId) {
                return $http.get("backoffice/ucop/Describe/Urls", {
                     params: {
                          contentTypeId: documentTypeId
                     }
                });
            }
        }
    }
);