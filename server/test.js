/**
 * Created by zhuangqh on 16/5/11.
 */

var mongodb = require('mongodb').MongoClient;
var debug = require('debug')('judu:temporary add feed');

var mongourl = 'mongodb://localhost:27017/judu';
mongodb.connect(mongourl).catch(function (error) {
  debug('Connect to ' + mongourl + ' was failed with error: ', error);
}).then(function (db) {
  var feed_generator = require('./models/feed_generator')(db);
  feed_generator.generateFeed();
  //db.close();
});
