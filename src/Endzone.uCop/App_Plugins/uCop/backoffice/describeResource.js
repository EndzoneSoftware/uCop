angular.module("umbraco.resources").factory("describeResource",
    function ($http) {
        return {
            getDocumentTypes: function () {
                return $http.get("backoffice/ucop/Describe/DocumentTypes");
            }
        }
    }
);