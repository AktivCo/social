angular.module("SocialPackage.services", [])

.value('common', {
    currentUser: null,
    categories: null

})


.factory('tmCredentials', ['$http', 'common', '$rootScope', function ($http, common, $rootScope) {
    var current = {};
    $http.get($rootScope.baseUrl + '/api/userdata/GetCategories').success(function (data) {
        common.categories = data;
    })



    $http.get($rootScope.baseUrl + '/api/userdata/getuserdata').success(function (data) {
        current.user = data;
        common.currentUser = data;
    })
    return {
        currentUser: current
    }

}])
