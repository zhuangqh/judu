var express = require('express');
var router = express.Router();
var fs = require('fs');
var path = require('path');
var url = require('url');
var debug = require('debug')('judu:index');

var avaibleList = "adaymag geekfan scipark appinn ifanr yidu dajia ifeng zhihu duxieren kr36 ecocn msa engadget";

module.exports = function (db) {
  var feeds = require('../models/feeds')(db);
  var users = require('../models/user')(db);

  /*
  // 获取特定的推送页面
  router.get('/', function(req, res, next) {
    console.log(req.query);
    var feedType = req.query["type"];

    if (avaibleList.indexOf(feedType) != -1  && req.query["id"]) {   // 确认所请求的推送是可用的
      var realPath = getFeedPath(feedType);


      fs.readFile(realPath, function (err, data) {
       var allFeeds = JSON.parse(data.toString());

       var id = parseInt(req.query["id"]);
       if (id >= 0 && id < allFeeds.length) {
       res.render('index', allFeeds[id]);
       } else {
       rejectReq(res);
       }
       });
    } else {
      rejectReq(res);
    }

  });
*/

  // 获取推送的overview
  router.get('/overview', function (req, res) {
    var feedType = req.query["type"];
    var feedList = [];

    if (feedType == "all") {   // 根据用户名获取他的所有订阅
      users.getSubscription(req.query["username"])
        .then(function (list) {
          return feeds.getFeed(list, getDate());
        }).then(function (doc) {
          res.send(doc);
        })
        .catch(function () {
          rejectReq(res);
        });
    } else if (avaibleList.indexOf(feedType) != -1) {    // 获取特定的订阅
      feedList.push(feedType);
      feeds.getFeed(feedList, getDate())
        .then(function (doc) {
          res.send(doc);
        })
        .catch(function () {
          rejectReq(res);
        });

    } else {
      rejectReq(res);
    }
  });

  // 根据推送的类型, 获取相应目录下根据时间编号的路径
  function getDate() {
    var date = new Date();
    var currentDate = date.getFullYear() + '_' + (date.getMonth() + 1) + '_' + date.getDate();
    return currentDate;
  }

  // 拒绝请求
  function rejectReq(res) {
    res.status(404);
    res.end();
  }

  return router;
};
