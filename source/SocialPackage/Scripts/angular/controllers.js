angular.module("SocialPackage.controllers", [])

.controller("MainCtrl", ["$scope", "tmCredentials", function ($scope, tmCredentials, common) {
    $scope.currentUser = tmCredentials.currentUser;
}])

.controller("ApproveModal", ["$scope", "$modalInstance", "$http", "bid", function ($scope, $modalInstance, $http, bid) {
    $scope.bid = bid;
    $scope.close = function () {
        $modalInstance.dismiss();
    }

    $scope.ShowBid = function(lBid)
    {
        $scope.bid = lBid;
    }

    $http.post($scope.baseUrl + "/api/Bids/GetLastBids", { UserId: bid.BidUser.id, CategoryId: bid.category.id }).success(function (data) {
        $scope.LastBids = data;
    })

    $scope.Approve = function () {
        console.log($scope.bid);
        $http.post($scope.baseUrl + "/api/Bids/Approve/", { bid: $scope.bid.id, comment: $scope.bid.Comment }).success(function (data) { document.location.href = $scope.baseUrl + "/Accountant"; });

    }

    $scope.Withdraw = function () {
        if ($scope.bid.Comment)
            $http.post($scope.baseUrl + "/api/Bids/Withdraw/", { bid: $scope.bid }).success(function (data) { document.location.href = $scope.baseUrl + "/Accountant"; });
        else {
            $scope.error = true;
        }
    }

}])

.controller("PayModal", ["$scope", "$modalInstance", "$http", "bid", function ($scope, $modalInstance, $http, bid) {
        $scope.bid = bid;
        $scope.close = function () {
            $modalInstance.dismiss();
        }

        $scope.Pay = function () {
            $http.post($scope.baseUrl + "/api/Bids/Pay/" + bid.id).success(function (data) { document.location.href = $scope.baseUrl + "/Accountant/Pay"; });

        }

        $scope.Withdraw = function () {
            if ($scope.bid.Comment)
                $http.post($scope.baseUrl + "/api/Bids/Withdraw/", { bid: $scope.bid }).success(function (data) { document.location.href = $scope.baseUrl + "/Accountant/Pay"; });
            else
                $scope.error = true;
        }

    }])


.controller("PayBid", ["$scope", "$http", "$modal", function ($scope, $http, $modal) {

    $scope.Data = {
        Date:new Date()
    }

    $scope.ShowApprove = function (bid) {
        var modal = $modal.open({
            templateUrl: 'pay.html',
            controller: 'PayModal',
            windowTemplateUrl: $scope.baseUrl + "/templates/ModalWindow.html",
            keyboard: false,
            size: 'lg',
            resolve: {
                bid: function () {
                    return bid;
                }
            }
        });
    }

    $scope.$watch("Data.Date", function () {
        
        
        $http.get($scope.baseUrl + "/api/Bids/GetBidsForPay?data=" + $scope.getFormatedDate($scope.Data.Date)).success(function (data) {
            
            $scope.Bids = data;
        });
    })

    
}])

.controller("SendedBidsCtrl", ["$scope", "$http", function ($scope, $http) {

    $scope.delete = function (id) {

        $http.get($scope.baseUrl + "/api/Bids/Delete/" + id).success(function () {
            document.location = $scope.baseUrl +"/Home/SendedBids";
        })
    }
}])

.controller("ApproveBid", ["$scope", "$http", "$modal", function ($scope, $http, $modal) {



    $scope.ShowApprove = function (bid) {
        var modal = $modal.open({
            templateUrl: 'approve.html',
            controller: 'ApproveModal',
            windowTemplateUrl: $scope.baseUrl + "/templates/ModalWindow.html",
            keyboard: false,
            size: 'lg',
            resolve: {
                bid: function () {
                    return bid;
                }
            }
        });
    }


    $http.get($scope.baseUrl + "/api/Bids/GetBidsForApprove").success(function (data) {

        $scope.BidsForApprove = data;
        
    });
}])

.controller("AddUsrCtrl", ["$scope", "$http", "$filter", "filterFilter", function ($scope, $http, $filter, filterFilter) {
    $scope.loginselect = '';
    $scope.availUsers = {};
    $scope.User = {};
    $scope.limitId;
    $scope.Alllimits;
    $scope.RealLimit;

    $scope.Init = function(model)
    {
        $scope.User = model;
    }

    $scope.GetAllLimits = function (allLimits)
    {
        $scope.Alllimits = allLimits;
    }
        
    $scope.LimitChange = function ()
    {
        var curLimit = filterFilter($scope.Alllimits, { Id: parseInt($scope.limitId) }, true)[0]
        $scope.RealLimit = angular.copy(curLimit)
    }

    $http.get($scope.baseUrl + '/api/admin/GetDomainUsers').success(function (data) {
        $scope.availUsers = data;        
    }).error(function(data) {
        $scope.userError = "Не удалось подключиться к домену. Проверьте указанные данные";
    
    });
    

}])

.controller("uploadModal", ["$scope", "filterFilter", "$filter", "$modalInstance", "$http", "common", "uploader", function ($scope, filterFilter, $filter, $modalInstance, $http, common, uploader) {
    var loader = uploader;
    // $scope.Categories = common.categories;
    //$scope.SelectedCat = {
    // category : $scope.Categories[0]
    //}
    $scope.form = {}
    $scope.image = uploader.queue[0]._file;
    

    $scope.ForUser = {
        user: "",
        Categories: []
    };


    $scope.getCategories = function () {
        $scope.Categories = $filter("availCategories")($scope.ForUser.user, common.categories);
        $scope.SelectedCat = {
            category: $scope.Categories[0]
        }
    }


    if (common.currentUser.Roles.indexOf("Секретарь") != -1 || common.currentUser.Roles.indexOf("Администратор") != -1) {
        $http.get($scope.baseUrl + "/api/userdata/GetAllUsers").success(function (data) {
            $scope.ChoseUsers = data;
            $scope.ForUser.user = filterFilter(data, { id: common.currentUser.id }, true)[0];
            $scope.getCategories()
        })
    }
    else {
        $scope.ForUser.user = common.currentUser;
        $scope.getCategories();
    }



    $scope.common = function (param) {
        return common[param];
    };

    $scope.Data =
        {
            Sum: "",
            Date: "",
        }

    
    $scope.close = function () {
        loader.clearQueue();
        $modalInstance.dismiss();
    }

    loader.onBeforeUploadItem = function (item) {
        item.formData = [];
        item.formData.push({ Sum: $scope.Data.Sum, Date: $scope.getFormatedDate($scope.Data.Date), CategoryId: $scope.SelectedCat.category.id, ForUserId: $scope.ForUser.user.id });
    };

    uploader.onCompleteItem = function (fileItem, response, status, headers) {

   
            switch (response.message) {
                case "No Limit":
                    $scope.error = "Вы превысили лимит. Вы можете запросить: " + response.availLimit;
                    $scope.Data.Sum = response.availLimit;
                    break;
                case "BadString":
                    $scope.error = "Введите корректную сумму";
                    break;
                case "BadDate":
                    $scope.error = "Дата не допустима";
                    break;
                default:
                    loader.clearQueue();
                    // $modalInstance.dismiss();
                    // $scope.close();
                    document.location.href = $scope.baseUrl + "/Home";
                    break;
            }


     
    };

    

    $scope.Save = function (event) {
        if ($scope.form.BidForm.$valid)
            uploader.queue[0].upload();
        // uploader.clearQueue();
        //$scope.close();
    }

}])


.controller("SettingsCtrl", ["$scope", "$http", "$modal", function ($scope, $http, $modal) {

    
    $http.get($scope.baseUrl + "/api/Bids/GetSettings/").success(function (data) {
        $scope.Settings = data;
    });

    $scope.Save = function () {
        $http.post($scope.baseUrl + "/Settings/Save/", $scope.Settings).error(function (data) {
            $scope.errors = data;
        });

    };

    
    $scope.SaveStart = function () {
        $http.post($scope.baseUrl + "/Settings/Save/", $scope.Settings)
            .error(function (data) {
                $scope.errors = data;
            })
            .success(function (data) {
                var modal = $modal.open({
                    templateUrl: 'AddUser.html',
                    //windowTemplateUrl: '/templates/ModalWindow.html',
                    controller: 'SettingsModalCtrl',
                    keyboard: false,
                    size: 'lg',
                    backdrop: 'static'                    
                });
            });

    };

    

}])

.controller("RulesModalCtrl", ["$scope", "$http", "$modalInstance", "settings", function ($scope, $http, $modalInstance, settings) {

    $scope.Settings = settings;

    $scope.close = function () {
        $modalInstance.dismiss();
    }
}])

.controller("FileUpload", ["$scope", "$http", "$modal", "FileUploader", function ($scope, $http, $modal, FileUploader) {
    var uploader = $scope.uploader = new FileUploader({
        url: $scope.baseUrl + "/Home/AddNewBid"
    })

    $http.get($scope.baseUrl + "/api/Bids/GetSettings/").success(function (data) {
        $scope.Settings = data;
    });

    $scope.ShowRules = function ()
    {
        var modal = $modal.open({
            templateUrl: 'RulesModal.html',
            //windowTemplateUrl: '/templates/ModalWindow.html',
            controller: 'RulesModalCtrl',
            keyboard: false,
            size: 'lg',

            backdrop: 'static',
            resolve: {
                settings: function () {
                    return $scope.Settings;
                }
            }
            
        });
    }

    $scope.delete = function (id) {

        $http.get($scope.baseUrl + "/api/Bids/Delete/" + id).success(function () {
            document.location = $scope.baseUrl + "/"
        })
    }


    uploader.filters.push({
        name: 'imageFilter',
        fn: function (item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });


    $scope.uploader.onAfterAddingFile = function (fileItem) {
        console.info('onAfterAddingFile', fileItem);
        var modal = $modal.open({
            templateUrl: 'uploadModal.html',
            windowTemplateUrl: $scope.baseUrl + '/templates/ModalWindow.html',
            controller: 'uploadModal',
            keyboard: false,
            size: 'lg',

            backdrop: 'static',
            resolve: {
                uploader: function () {
                    return uploader;
                }
            }
        });
    };




}])

.controller("SettingsModalCtrl", ["$scope", "$http", "$modalInstance", function ($scope, $http, $modalInstance) {
    
    $scope.loginselect = '';
    $scope.availUsers = {};
    $scope.User = {};

    $scope.Init = function (model) {
        $scope.User = model;
    }

    

    $http.get($scope.baseUrl + '/api/admin/GetDomainUsers').success(function (data) {
        $scope.availUsers = data;
    }).error(function (data) {
        $scope.userError = "Не удалось подключиться к домену. Проверьте указанные данные";

    });

    $scope.close = function () {
        $modalInstance.dismiss();
    }
}])