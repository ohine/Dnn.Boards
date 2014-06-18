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

dnn.boards.Board = function ($, ko, settings, moduleScope) {
    $.extend(this, dnn.boards.Board.defaultSettings, settings);
    
    // custom binding handler
    ko.bindingHandlers.flash = {
        init: function(element) {
            $(element).hide();
        },
        update: function(element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            if (value) {
                $(element).stop().hide().text(value).fadeIn(function() {
                    clearTimeout($(element).data("timeout"));
                    $(element).data("timeout", setTimeout(function() {
                        $(element).fadeOut();
                        valueAccessor()(null);
                    }, 3000));
                });
            }
        },
        timeout: null
    };

    // card model
    var card = function(item) {
        var self = this;

        self.id = item.Id;
        self.cardId = item.CardId;
        self.title = ko.observable(item.Title);
        self.contentTitle = ko.observable(item.ContentTitle);
        self.content = ko.observable(item.Content);
        self.contentItemId = item.ContentItemId;
        self.dueDate = ko.observable(item.DueDate);
        self.comments = ko.observable(item.Comments);
        self.openItems = ko.observable(item.OpenItems);
        self.completedItems = ko.observable(item.CompletedItems);
        self.labels = ko.observable(item.Labels);
        self.members = ko.observable(item.Members);
        self.boardListId = ko.observable(item.BoardListId);

        // a variable used to determine what, if anything, is being edited
        self.view = ko.observable('read');

        // title actions
        self.editTitle = function() {
            self.view('title');
        },
        self.cancelEditTitle = function() {
            self.view('read');
        },
        self.saveTitle = function(updatedCard) {
            var title = updatedCard.title();

            if (title.length > 0) {
                self.serviceCaller = new dnn.boards.ServiceCaller($, this.moduleId, 'BoardsService');

                var success = function (model) {
                    if (model.success = "true") {
                        self.view('read');
                    }
                    console.log('success', model);
                };

                var failure = function (response, status) {
                    console.log('request failure: ' + status);
                };

                var params = {
                    cardId: updatedCard.cardId,
                    title: title
                };

                self.serviceCaller.post('UpdateCardTitle', params, success, failure);
            }
        };

        // description actions
        self.editDescription = function() {
            self.view('description');
        },
        self.cancelEditDescription = function() {
            self.view('read');
        },
        self.saveDescription = function(updatedCard) {
            var content = updatedCard.content();

            if (content.length > 0) {
                self.serviceCaller = new dnn.boards.ServiceCaller($, this.moduleId, 'BoardsService');

                var success = function (model) {
                    if (model.success = "true") {
                        self.view('read');
                    }
                    console.log('success', model);
                };

                var failure = function (response, status) {
                    console.log('request failure: ' + status);
                };

                var params = {
                    cardId: updatedCard.cardId,
                    content: content
                };

                self.serviceCaller.post('UpdateCardDescription', params, success, failure);
            }
        };
    };

    // board list model
    var list = function(item) {
        var self = this;

        self.name = ko.observable(item.Name);
        self.archived = ko.observable(item.Archived);
        self.boardListId = ko.observable(item.BoardListId);
        self.boardId = this.boardId;

        if (typeof item.Cards !== "undefined" && item.Cards != null) {
            var results = [];
            ko.utils.arrayForEach(item.Cards, function(listCard) {
                results.push(new card(listCard));
            });
            self.cards = ko.observableArray(results);
        } else {
            self.cards = ko.observableArray();
        }

        self.cards.listId = item.BoardListId;
        self.cards.listName = item.Name;

        // ensure only the first list shows the compose area
        self.showAddCard = !!(item.SortOrder == 0);

        // actions
        self.removeCard = function (thisCard) {
            self.serviceCaller = new dnn.boards.ServiceCaller($, this.moduleId, 'BoardsService');

            var success = function (model) {
                if (model.success = "true") {
                    self.cards.remove(thisCard);
                    $("#divModal").dialog("close");  
                }
                console.log('success', model);
            };

            var failure = function (response, status) {
                console.log('request failure: ' + status);
            };

            var params = {
                cardId: thisCard.cardId,
                boardListId: thisCard.boardListId(),
                contentItemId: thisCard.contentItemId
            };

            self.serviceCaller.post('DeleteCard', params, success, failure);
        };
        self.createCard = function() {
            var title = $("#textTitle").val().trim();
            if (title.length > 0) {
                self.serviceCaller = new dnn.boards.ServiceCaller($, this.moduleId, 'BoardsService');

                var success = function (model) {
                    if (success = "true") {
                        var newCard = new card(model.newCard);
                        self.cards.push(newCard);
                        // clear data
                        $("#textTitle").val('');
                    }
                    console.log('success', model);
                };

                var failure = function (response, status) {
                    console.log('request failure: ' + status);
                };

                var params = {
                    BoardListId: self.boardListId(),
                    Title: title
                };

                self.serviceCaller.get('CreateCard', params, success, failure);
            }
        };
    };

    // The view model is an abstract description of the state of the UI, but without any knowledge of the UI technology (HTML)
    var boardViewModel = function(initialData) {
        var self = this;

        var initialLists = [];
        ko.utils.arrayForEach(initialData, function(item) {
            initialLists.push(new list(item));
        });
        self.lists = ko.observableArray(initialLists);

        // determines which card, if any, is selected for editing
        self.selectedCard = ko.observable();

        // variables for showing user interaction
        self.lastAction = ko.observable();
        self.lastError = ko.observable();

        // actions
        self.openCard = function(cardToEdit) {
            self.selectedCard(cardToEdit);
            $("#divModal").dialog("open");
            //	        $("#divModal").parent().appendTo($("form:first"));
        };

        // something was successfully dropped
        self.updateLastAction = function(arg) {

            if (arg.sourceParent.listName == arg.targetParent.listName) {
                // we changed sort order
                self.serviceCaller = new dnn.boards.ServiceCaller($, this.moduleId, 'BoardsService');

                var success = function (model) {
                    if (model.success = "true") {
                        var response = "Moved '" + arg.item.title() + "' from position " + (arg.sourceIndex + 1) + " to position " + (arg.targetIndex + 1);
                        // show message to end user
                        self.lastAction(response);
                    }
                    console.log('success', model);
                };

                var failure = function (response, status) {
                    console.log('request failure: ' + status);
                };

                var params = {
                    cardId: arg.item.cardId,
                    sortOrder: arg.targetIndex
                };

                self.serviceCaller.post('UpdateCardOrder', params, success, failure);
            } else {
                // we changed the board list id
                self.serviceCaller = new dnn.boards.ServiceCaller($, this.moduleId, 'BoardsService');

                var success2 = function (model) {
                    if (model.success = "true") {
                        var response = "Moved '" + arg.item.title() + "' from '" + arg.sourceParent.listName + "' to '" + arg.targetParent.listName + "'";
                        // show message to end user
                        self.lastAction(response);
                    }
                    console.log('success', model);
                };

                var failure2 = function (response, status) {
                    console.log('request failure: ' + status);
                };

                var params2 = {
                    cardId: arg.item.cardId,
                    boardListId: arg.targetParent.listId,
                    sortOrder: arg.targetIndex
                };

                self.serviceCaller.post('UpdateCardList', params2, success2, failure2);
            }
        };

        // verify user is permitted to drop (apply custom logic here)
        self.verifyAssignments = function(arg) {
            // var response = "";
            // self.lastError(response);
        };

        // show/hide compose card area
        self.expandCompose = function() {
            $("#composeArea").show();
            $(".hlCompose").hide();
        };
        self.collapseCompose = function() {
            $("#composeArea").hide();
            $(".hlCompose").show();
        };
    };

    // we call this to initialize (from the .ascx file)
    this.init = function () {
        var that = this;
        
        that.serviceCaller = new dnn.boards.ServiceCaller($, this.moduleId, 'BoardsService');
        
        var success = function (model) {
            if (typeof model !== "undefined" && model != null) {
                var viewModel = new boardViewModel(model.colLists);

                ko.bindingHandlers.sortable.beforeMove = viewModel.verifyAssignments;
                ko.bindingHandlers.sortable.afterMove = viewModel.updateLastAction;

                // normally, we apply moduleScope as a second parameter
                ko.applyBindings(viewModel);
            }

            console.log('success', model);
        };
 
        var failure = function(response, status) {
            console.log('request failure: ' + status);
        };

        var params = {
            boardId: this.boardId
        };

        that.serviceCaller.get('GetBoardLists', params, success, failure);

        // setup 
        $("#divModal").dialog({ modal: true, autoOpen: false, dialogClass: "dnnFormPopup", minWidth: 485 });
    };
    
};

dnn.boards.Board.defaultSettings = {
    boardId: -1,
    moduleId: 10,
    moduleScope: $("#divBoard")[0]
};