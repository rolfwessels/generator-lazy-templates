var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var minifyCss = require('gulp-minify-css');
var jshint = require('gulp-jshint');
var del = require('del');
var runSequence = require('run-sequence');
var argv = require('yargs').argv;
var browserSync = require('browser-sync');

config = {
    src: {
        site: 'MainSolutionTemplate.Website',
        js: [
            'MainSolutionTemplate.Website/scripts/**/*.js',
            '!MainSolutionTemplate.Website/scripts/dist/*.js',
        ],
        vendor: [
            'MainSolutionTemplate.Website/bower_components/jquery/dist/jquery.js',
            'MainSolutionTemplate.Website/bower_components/angular/angular.js',
            'MainSolutionTemplate.Website/bower_components/angular-route/angular-route.js',
            'MainSolutionTemplate.Website/bower_components/angular-animate/angular-animate.js',
            'MainSolutionTemplate.Website/bower_components/angular-local-storage/dist/angular-local-storage.js',
            'MainSolutionTemplate.Website/bower_components/angular-loading-bar/src/loading-bar.js',
            'MainSolutionTemplate.Website/bower_components/signalr/jquery.signalr.js',
            'MainSolutionTemplate.Website/bower_components/materialize/bin/materialize.js',
            'MainSolutionTemplate.Website/bower_components/angular-materialize/src/angular-materialize.js'
        ],
        css: [
            'MainSolutionTemplate.Website/bower_components/materialize/bin/materialize.css',
            'MainSolutionTemplate.Website/assets/css/app.css',
        ],
        dist: [
            'MainSolutionTemplate.Website/views/**/*',
            'MainSolutionTemplate.Website/*.html',
            'MainSolutionTemplate.Website/assets/**/*',
            'MainSolutionTemplate.Website/scripts/dist/**/*',
            'MainSolutionTemplate.Website/*.ico'
        ]
    },
    dest: {
        js: 'MainSolutionTemplate.Website/scripts/dist/',
        css: 'MainSolutionTemplate.Website/assets/css/',
        dist: argv.output || 'MainSolutionTemplate.Website/dist/'
    }
};

//
// Main tasks
//

gulp.task('default', ['scripts', 'vendor', 'minify-css']);

gulp.task('watch', function() {
    gulp.watch(config.src.js, ['scripts', 'jshint']);
    gulp.watch(config.src.css, ['minify-css']);
});


gulp.task('serve', function() {
  return runSequence(['serve.site','watch']);
});


gulp.task('dist', function() {
    return runSequence('default', 'dist.clean', 'dist.copy');
});


//
// sub tasks
//

gulp.task('dist.clean', function(cb) {
    return del(config.dest.dist, cb);
});

gulp.task('dist.copy', function() {
    return gulp.src(config.src.dist, {
            base: "./MainSolutionTemplate.Website"
        })
        .pipe(gulp.dest(config.dest.dist));
});

gulp.task('serve.site', function() {
  browserSync({
    server: {
      baseDir: config.src.site
    }
  });

  gulp.watch(['views/**/*.html','*.html', 'assets/css/**/*.css', 'scripts/dist/**/*.js'], {cwd: config.src.site}, browserSync.reload);
});

gulp.task('scripts', function() {
    return gulp.src(config.src.js)
        .pipe(concat('scripts.js'))
        .pipe(gulp.dest(config.dest.js));
});

gulp.task('jshint', function() {
    return gulp.src(config.src.js)
        .pipe(jshint('.jshintrc'))
        .pipe(jshint.reporter('default'));
});

gulp.task('vendor', function() {
    return gulp.src(config.src.vendor)
        .pipe(concat('vendor.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest(config.dest.js))
});

gulp.task('minify-css', function() {
    return gulp.src(config.src.css)
        .pipe(concat('app.min.css'))
        .pipe(minifyCss({
            compatibility: 'ie8'
        }))
        .pipe(gulp.dest(config.dest.css));
});
