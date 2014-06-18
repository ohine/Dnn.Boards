//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2012
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

//ensure namespace
dnn.boards = dnn.boards || {};

dnn.boards.ServiceCaller = function ($, moduleId, controller) {
    var that = this;

    this.services = $.dnnSF(moduleId);

    this.getRoot = function () {
        return that.services.getServiceRoot('DNNCorp/Boards') + controller + '/';
    };

    this.getModuleId = function () {
        return that.services.getModuleId();
    };

    this.getTabId = function () {
        return that.services.getTabId();
    };

    this.privateCall =
        function (httpMethod, method, params, success, failure, synchronous) {
            var options = {
                url: that.getRoot() + method,
                beforeSend: that.services.setModuleHeaders,
                type: httpMethod,
                async: synchronous == false,
                success: function (d) {
                    if (typeof (success) != 'undefined') {
                        success(d || {});
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (typeof (failure) != 'undefined') {
                        var message = undefined;

                        if (xhr.getResponseHeader('Content-Type').indexOf('application/json') == 0) {
                            try {
                                message = $.parseJSON(xhr.responseText).Message;
                            } catch (e) {
                            }
                        }

                        failure(xhr, message || errorThrown);
                    }
                }
            };

            if (httpMethod == 'GET') {
                options.data = params;
            }
            else {
                options.contentType = 'application/json; charset=utf-8';
                options.data = ko.toJSON(params);
                options.dataType = 'json';
            }

            $.ajax(options);
        };

    this.post = function (method, params, success, failure) {
        that.privateCall('POST', method, params, success, failure, false);
    };

    this.postsync = function (method, params, success, failure) {
        that.privateCall('POST', method, params, success, failure, true);
    };

    this.get = function (method, params, success, failure) {
        that.privateCall('GET', method, params, success, failure, false);
    };

    this.getsync = function (method, params, success, failure) {
        that.privateCall('GET', method, params, success, failure, true);
    };
};