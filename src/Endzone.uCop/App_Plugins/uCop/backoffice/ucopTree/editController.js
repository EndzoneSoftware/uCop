angular.module("umbraco").controller("uCop.Structure.EditController",
    function ($scope, navigationService, describeResource) {
        var hierarchy = {};

        function sortAlphabeticallyByDocTypeName(a, b) {
            return (a.docType.name < b.docType.name) ? -1 : (a.docType.name > b.docType.name) ? 1 : 0;
        }

        function reorderHierarchically() {
            function addNode(c) {
                $scope.documentTypes.push(c);
                c.children.forEach(addNode);
            }

            $scope.documentTypes = [];
            var rootNode = hierarchy[-1];
            if (rootNode) {
                rootNode.children.forEach(addNode);
            }
        };

        function sort() {
            switch ($scope.sort.mode) {
                case "Hierarchy":
                    reorderHierarchically();
                    break;

                default:
                    $scope.documentTypes.sort(sortAlphabeticallyByDocTypeName);
            }
        }

        function buildLinks(documentType, urls, loaded) {
            documentType.links = {
                loaded: loaded,
                urls: urls,
                unpublishedCount: urls.filter(function (u) { return !u.published && !u.trashed; }).length,
                trashedCount: urls.filter(function (u) { return u.trashed; }).length
            }    
        }

        $scope.loaded = false;      
        $scope.documentTypes = [];

        $scope.tabs = [{ id: "overview", label: "Overview" }];

        $scope.sort = {
            options: ["Alphabetical", "Hierarchy"],
            mode: "Hierarchy"
        }

        $scope.refresh = function () {
            $scope.loaded = false;
            hierarchy = {};

            describeResource.getDocumentTypes().then(function (response) {
                $scope.documentTypes = response.data;
                $scope.documentTypes.forEach(function(dt) {
                    buildLinks(dt, dt.urls.sample, dt.urls.totalCount == dt.urls.sample.length);
                    dt.links.totalCount = dt.urls.totalCount;
                    delete dt.urls;

                    dt.children = [];
                    hierarchy[dt.docType.id] = angular.extend(dt, hierarchy[dt.docType.id]);
                    if (!hierarchy.hasOwnProperty(dt.docType.parentId)) {
                        hierarchy[dt.docType.parentId] = {
                            children: []
                        }
                    }
                    var parent = hierarchy[dt.docType.parentId];
                    parent.children.push(dt);
                    parent.children.sort(sortAlphabeticallyByDocTypeName);
                });

                sort();

                $scope.loaded = true;
            });
        };

        $scope.getDocumentTypeParentHierarchyAsArray = function (documentType) {
            if (documentType === undefined)
                return [];

            var result = [];
            while (documentType.docType.parentId > -1) {
                documentType = hierarchy[documentType.docType.parentId];
                result.push(documentType);
            }
            result.reverse();
            return result;
        }

        $scope.navigateToDocumentType = function (id) {
            navigationService.changeSection("settings");
            openNodeType(id); //from LegacyTreeJs
        }

        $scope.navigateToTemplate = function (id) {
            navigationService.changeSection("settings");
            openTemplate(id); //from LegacyTreeJs
        }

        $scope.loadUrls = function (documentType) {
            describeResource.getUrls(documentType.docType.id).then(function(response) {
                buildLinks(documentType, response.data, true);
            });
        }

        $scope.$watch("sort.mode", function (newValue, oldValue) {
            if (newValue !== oldValue) {
                sort();
            }
        });

        $scope.refresh();
    });
