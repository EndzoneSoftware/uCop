angular.module("umbraco").controller("uCop.Structure.EditController",
    function ($scope, describeResource) {
        $scope.loaded = false;      
        $scope.documentTypes = [];

        $scope.tabs = [{ id: "overview", label: "Overview" }];

        $scope.refresh = function () {
            $scope.loaded = false;
            
            describeResource.getDocumentTypes().then(function (response) {
                $scope.documentTypes = response.data;
                $scope.documentTypes.forEach(function(dt) {
                    dt.links = {
                        urls: dt.urls,
                        unpublishedCount: dt.urls.filter(function (u) { return !u.published && !u.trashed; }).length,
                        trashedCount: dt.urls.filter(function (u) { return u.trashed; }).length
                    }
                    delete dt.urls;
                });
                $scope.loaded = true;
            });
        };

        $scope.refresh();
    });
