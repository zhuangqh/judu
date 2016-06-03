/**
 * Created by zhuangqh on 16/5/11.
 */

var debug = require('debug')('judu:feeds');

module.exports = function (db) {
  debug("database feeds work as normal");
  var feeds = db.collection("feeds");

  function mergeFeed(allFeed) {
    // 获取消息源的概览
    function overview(feedItem, feedType) {
      var i;
      for (i = 0; i < feedItem.length; ++i) {
        delete feedItem[i].description;
        feedItem[i].feedType = feedType;
      }
      return feedItem;
    }

    var i = 0;
    var ans = [];
    for (; i < allFeed.length; i++) {
      ans = ans.concat(overview(allFeed[i].feed, allFeed[i].feedType));
    }
    return ans;
  }
  return {
    /*
     * feedList: feedType, feedDate
     */
    getFeed: function (feedList, date) {
      return feeds.find({'feedType': {'$in': feedList}, 'feedDate': date})
        .toArray().then(function (doc) {
          return doc ? Promise.resolve(mergeFeed(doc)) : Promise.reject();
        });
    },

    /*
     * feed: feedType, feedDate, feed
     */
    addFeed: function (feedItem) {
      return feeds.find({'feedType': feedItem.feedType, 'feedDate': feedItem.feedDate})
        .toArray().then(function (doc) {
          if (doc.length <= 0)
            feeds.insertOne(feedItem);
        });
    }
  };
};
