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
            'scripts/**/*.js'
        ],
        
        vendor: [
            'bower_components/jquery/dist/jquery.js',
            'bower_components/angular/angular.js',
            'bower_components/angular-route/angular-route.js',
            'bower_components/angular-animate/angular-animate.js',
            'bower_components/angular-local-storage/dist/angular-local-storage.js',
            'bower_components/angular-loading-bar/build/loading-bar.js',
            'bower_components/signalr/jquery.signalr.js',
            'bower_components/materialize/bin/materialize.js',
            'bower_components/angular-materialize/src/angular-materialize.js'
        ],
        css: [
            'bower_components/materialize/bin/materialize.css',
            'bower_components/angular-loading-bar/build/loading-bar.min.css',
            'assets/css/app.css'
        ],
        baseHtml: "./",
        html: [
            'views/**/*',
            '*.html',
            '*.ico'
        ],
        assets: [
            'assets/image*/**/*',
            'bower_components/materialize/font*/**/*'
        ]
    },
    dest: {
        css: 'assets/css/',
        dist: argv.output || ('dist')
    }
};



//
// Main tasks
//

gulp.task('default', ['build']);



gulp.task('watch', function() {
    gulp.watch(config.src.js, ['build.scripts', 'jshint']);
    gulp.watch(config.src.css, ['build.css']);
    gulp.watch(config.src.html, ['build.html']);
    gulp.watch(config.src.assets, ['build.assets']);
});


gulp.task('serve', function () {
  return runSequence(['serve.site','watch']);
});


gulp.task('build', function() {
    return runSequence('build.clean', ['build.html', 'build.css', 'build.scripts', 'build.vendor', 'build.assets']); 
});

gulp.task('dist', function() {
    return runSequence('set.dist', ['build']); 
});


//
// sub tasks
//

gulp.task('build.clean', function (cb) {
    return del(config.dest.dist, cb);
});

gulp.task('build.html', function () {
    return gulp.src(config.src.html, { base: config.src.baseHtml })
        .pipe(gulp.dest(config.dest.dist));
});

gulp.task('build.scripts', function () {
    return gulp.src(config.src.js)
        .pipe(concat('scripts.js'))
        .pipe(gulp.dest(config.dest.dist + "/scripts"));
});

gulp.task('build.vendor', function () {
    return gulp.src(config.src.vendor)
        .pipe(concat('vendor.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest(config.dest.dist + "/scripts"));
});

gulp.task('build.css', function () {
    return gulp.src(config.src.css)
        .pipe(concat('app.min.css'))
        .pipe(minifyCss({
            compatibility: 'ie8'
        }))
        .pipe(gulp.dest(config.dest.dist + "/assets/css/"));
});

gulp.task('build.assets', function () {
    return gulp.src(config.src.assets, { base: "./MainSolutionTemplate.Website" })
        .pipe(gulp.dest(config.dest.dist + "/assets/"));
});

gulp.task('build.assets', function () {
    return gulp.src(config.src.assets)
        .pipe(gulp.dest(config.dest.dist + "/assets/"));
});

gulp.task('set.dist', function () {
    config.dest.dist = "dist";
});


gulp.task('serve.site', function() {
  browserSync({
    server: {
      baseDir: config.dest.dist
    }
  });

  gulp.watch(['views/**/*.html','*.html', 'assets/css/**/*.css', 'scripts/dist/**/*.js'], {cwd: config.src.site}, browserSync.reload);
});

gulp.task('jshint', function() {
    return gulp.src(config.src.js)
        .pipe(jshint('.jshintrc'))
        .pipe(jshint.reporter('default'));
});

