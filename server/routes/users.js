var express = require('express');
var router = express.Router();
var validator = require('../public/javascripts/validator');
var debug = require('debug')('judu:user');

module.exports = function (db) {
  var userManager = require('../models/user')(db);
  // 登录
  router.post('/signin', function (req, res) {
    debug(req.body);
    userManager.findUser(req.body.username, req.body.password)
      .then(function () {
        res.json({'errCode': 0});
      })
      .catch(function () {
        res.json({'errCode': 1, 'errMsg': '用户名密码错误'});
      });
  });

  // 提交注册信息
  // 防止脚本绕过客户端验证
  // 进行服务器端验证
  router.post('/regist', function (req, res) {
    debug(req.body);
    var user = req.body;
    user.subscription = [];
    user.favorite = [];
    userManager.checkUser(user)
      .then(userManager.createUser(user))
      .then(function () {
        res.json({'errCode': 0});
      })
      .catch(function (error) {
        debug('has error in regist: post');
        res.json({'errCode': 1, 'errMsg': '用户名已存在'});
      });
  });

  // 更新订阅源
  /*
   * user: username, digest: list<string>, favorite: list<string>
   */
  router.post('/profile', function (req, res) {
    var user = JSON.parse(req.body.user);
    debug(user);
    userManager.updateProfile(user)
      .then(function () {
        res.json({'errCode': 0});
      })
      .catch(function () {
        res.json({'errCode': 1, 'errMsg': '更新数据失败'});
      })
  });

  router.get('/profile', function (req, res) {
    var username = req.query.username;
    debug(req.query);

    userManager.getProfile(username)
      .then(function (profile) {
        res.json({'errCode': 0, 'date': profile});
      })
      .catch(function () {
        res.json({'errCode': 1, 'errMsg': '获取个人信息失败,请重试'});
      })
  });

  debug('router user work as normal');
  return router;
};
