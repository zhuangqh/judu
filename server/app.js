var express = require('express');
var path = require('path');
var favicon = require('serve-favicon');
var logger = require('morgan');
var debug = require('debug')('judu:app');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var cronJob = require('cron').CronJob;

module.exports = function (db) {
  debug('app setup');
  var routes = require('./routes/index')(db);
  var users = require('./routes/users')(db);

  var app = express();

  // view engine setup
  app.set('views', path.join(__dirname, 'views'));
  app.set('view engine', 'jade');

  // uncomment after placing your favicon in /public
  //app.use(favicon(path.join(__dirname, 'public', 'favicon.ico')));
  app.use(logger('dev'));
  app.use(bodyParser.json());
  app.use(bodyParser.urlencoded({ extended: false }));
  app.use(cookieParser());
  app.use(express.static(path.join(__dirname, 'public')));

  app.use('/', routes);
  app.use('/api', users);

  var feed_generator = require('./models/feed_generator')(db);

  var job = new cronJob('00 00 00 * * 0-6', function() {
      feed_generator.generateFeed();
    }, function () {
      /* This function is executed when the job stops */
      debug('feed generator stop');
    },
    true, /* Start the job right now */
    null /* Time zone of this job. */
  );

  // catch 404 and forward to error handler
  app.use(function(req, res, next) {
    var err = new Error('Not Found');
    err.status = 404;
    next(err);
  });

  // error handlers

  // development error handler
  // will print stacktrace
  if (app.get('env') === 'development') {
    app.use(function(err, req, res, next) {
      res.status(err.status || 500);
      res.render('error', {
        message: err.message,
        error: err
      });
    });
  }

  // production error handler
  // no stacktraces leaked to user
  app.use(function(err, req, res, next) {
    res.status(err.status || 500);
    res.render('error', {
      message: err.message,
      error: {}
    });
  });

  return app;
};
