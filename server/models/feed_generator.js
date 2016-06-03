/**
 * Created by zhuangqh on 16/5/4.
 */

/**
 * Tips
 * ====
 * - Set `user-agent` and `accept` headers when sending requests. Some services will not respond as expected without them.
 * - Set `pool` to false if you send lots of requests using "request" library.
 */

var request = require('request'),
    FeedParser = require('feedparser'),
    Iconv = require('iconv').Iconv,
    fs = require('fs'),
    debug = require('debug')('judu:feed_generator');


var feedUrlTable = {
    'zhihu': 'http://www.zhihu.com/rss',
    'scipark': 'http://www.scipark.net/feed/',
    'engadget': 'http://cn.engadget.com/rss.xml',
    'msa': 'http://blog.sina.com.cn/rss/1286528122.xml',
    'appinn': 'http://feeds.appinn.com/appinns/',
    'kr36': 'http://www.36kr.com/feed',
    'ifanr': 'http://www.ifanr.com/feed',
    'geekfan': 'http://www.geekfan.net/feed/',
    'dajia': 'http://hanhanone.sinaapp.com/feed/dajia',
    'yidu': 'http://yidu.im/rss',
    'ecocn': 'http://blog.ecocn.org/feed',
    'ruanyifeng': 'http://www.ruanyifeng.com/blog/atom.xml',
    'adaymag': 'http://www.adaymag.com/feed/',
    'ifeng': 'http://news.ifeng.com/rss/index.xml'
};

module.exports = function (db) {
    var feeds = require('./feeds')(db);
    debug("feed generator works as normal");

    function fetch(feedUrl, feedName) {
        var allFeed = [];
        // Define our streams
        var req = request(feedUrl, {timeout: 10000, pool: false});
        req.setMaxListeners(50);
        // Some feeds do not respond without user-agent and accept headers.
        req.setHeader('user-agent', 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36');
        req.setHeader('accept', 'text/html,application/xhtml+xml');

        var feedparser = new FeedParser();

        // Define our handlers
        req.on('error', function (err) {
            if (err) {
                debug(err);
            }
        });
        req.on('response', function(res) {
            if (res.statusCode != 200) return this.emit('error', new Error('Bad status code'));
            var charset = getParams(res.headers['content-type'] || '').charset;
            res = maybeTranslate(res, charset);
            // And boom goes the dynamite
            res.pipe(feedparser);
        });

        feedparser.on('error', function () {
            debug("fail to get " + feedName);
        });
        feedparser.on('end', function () {
            saveFeed(allFeed, feedName);
        });
        var c = 1;
        feedparser.on('readable', function() {
            var post, feed;
            while (post = this.read()) {
                //if (c < 2) console.log(post);
                //c++;
                feed = {
                    'title': post['title'],
                    'description': post['description'],
                    'pubDate': post['pubDate'],
                    'guid': post['guid'],
                    'author': post['author']
                };
                allFeed.push(feed);
            }
        });
    }

    function maybeTranslate (res, charset) {
        var iconv;
        // Use iconv if its not utf8 already.
        if (!iconv && charset && !/utf-*8/i.test(charset)) {
            try {
                iconv = new Iconv(charset, 'utf-8');
                console.log('Converting from charset %s to utf-8', charset);
                iconv.on('error', done);
                // If we're using iconv, stream will be the output of iconv
                // otherwise it will remain the output of request
                res = res.pipe(iconv);
            } catch(err) {
                res.emit('error', err);
            }
        }
        return res;
    }

    function getParams(str) {
        var params = str.split(';').reduce(function (params, param) {
            var parts = param.split('=').map(function (part) { return part.trim(); });
            if (parts.length === 2) {
                params[parts[0]] = parts[1];
            }
            return params;
        }, {});
        return params;
    }

    function saveFeed(allFeed, feedName) {
        var date = new Date();
        var saveDate = date.getFullYear() + '_' + (date.getMonth()+1) + '_' + date.getDate();

        debug('Add feed: ' + feedName + ' ' + saveDate);
        feeds.addFeed({'feedType': feedName, 'feedDate': saveDate, 'feed': allFeed});
    }

    return {
        generateFeed: function() {
            for (var feedType in feedUrlTable) {
                debug('start generate ' + feedType);
                if (feedUrlTable.hasOwnProperty(feedType)) {
                    fetch(feedUrlTable[feedType], feedType);
                }
            }
        }
    };
};
