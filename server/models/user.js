/**
 * Created by zhuangqh on 16/5/7.
 */

var bcrypt = require('bcrypt-nodejs');
var validator = require('../public/javascripts/validator');
var debug = require('debug')('judu:db');


module.exports = function (db) {
  debug("database user work as normal");
  var users = db.collection('users');

  return {
    findUser: function (username, password) {
      return users.findOne({username: username}).then(function (user) {
        if (user) { // 若用户存在，进行密码验证
          return new Promise(function (resolve, reject) {
            bcrypt.compare(password, user.password, function (err, res) {
              debug('result is :', res);
              return res ? resolve(user) : reject();
            });
          });
        } else {
          return Promise.reject("user doesn't exist");
        }
      });
    },

    // 创建user，并加密密码，返回promise
    createUser: function (user) {
      return function () {
        return new Promise(function (resolve, reject) {
          bcrypt.hash(user.password, null, null, function (err, result) {
            user.password = result;
            return users.insert(user).then(function () {
              return resolve();
            });
          });
        });
      };
    },

    // 查询用户是否已存在, 返回promise
    checkUser: function (user) {
      debug(user);
      return users.findOne({"username": user.username}).then(function (existedUser) {
          debug('existed user: ', existedUser);
          return existedUser ? Promise.reject() : Promise.resolve(user);
      });
    },

    getProfile: function (username) {
      return users.find({'username': username})
        .toArray().then(function (doc) {
          if (doc.length <= 0) {
            return Promise.reject();
          } else {
            delete doc[0]._id;
            delete doc[0].password;
            return Promise.resolve(doc[0]);
          }
        });
    },

    updateProfile: function (user) {
      if (user == null) {
        return Promise.reject();
      } else {
        return users.updateOne({"username": user.username}, {'$set': user});
      }
    }
  };

};
