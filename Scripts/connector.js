"use strict";
define(['jquery',
    'knockout'],
    function ($, ko) {
        var bindViewModel,
            rootFolder,
            utility,
            helper;

        var init = function (koObject, connectionHelper, pluginFolder, util) {
            bindViewModel = koObject;
            rootFolder = pluginFolder;
            utility = util;
            helper = connectionHelper;
        };

        var onAfterBind = function () {
        }

        var onSave = function (koObject) {
        }

        return {
            init: init,
            onSave: onSave
        }
    });
