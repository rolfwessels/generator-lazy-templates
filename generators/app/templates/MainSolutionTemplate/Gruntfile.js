module.exports = function(grunt) {

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        concat: {
            options: { 'separator': ';' },
            scripts: {
                'src': [
                    'src/MainSolutionTemplate.Website/scripts/**/*.js',
                    '!src/MainSolutionTemplate.Website/scripts/dist/*.js',
                ],
                'dest': 'src/MainSolutionTemplate.Website/scripts/dist/scripts.js'
            },
            vendor: {

                'src': [
                    'src/MainSolutionTemplate.Website/bower_components/angular/angular.js',
                    'src/MainSolutionTemplate.Website/bower_components/angular-route/angular-route.js',
                    'src/MainSolutionTemplate.Website/bower_components/angular-aria/angular-aria.js',
                    'src/MainSolutionTemplate.Website/bower_components/angular-animate/angular-animate.js',
                    'src/MainSolutionTemplate.Website/bower_components/angular-material/angular-material.js',
                    'src/MainSolutionTemplate.Website/bower_components/angular-local-storage/dist/angular-local-storage.js',
                    'src/MainSolutionTemplate.Website/bower_components/angular-loading-bar/src/loading-bar.js',
                    'src/MainSolutionTemplate.Website/bower_components/jquery/dist/jquery.js',
                    'src/MainSolutionTemplate.Website/bower_components/signalr/jquery.signalr.js',
                ],
                'dest': 'src/MainSolutionTemplate.Website/scripts/dist/vendor.js'
            }
           
        },
        uglify: {
          options: {
            banner: "/*! <%= pkg.name %> <%= grunt.template.today('yyyy-mm-dd') %> */\n"
          },
          scripts: {
            files: {
              'src/MainSolutionTemplate.Website/scripts/dist/scripts.min.js': ['<%= concat.scripts.dest %>']
            }
          },
          vendor: {
            files: {
              'src/MainSolutionTemplate.Website/scripts/dist/vendor.min.js': ['<%= concat.vendor.dest %>']
            }
          }
        },
        jshint: {
          files: ['Gruntfile.js', 'src/MainSolutionTemplate.Website/scripts/**/*.js' , '!src/MainSolutionTemplate.Website/scripts/*dist*.js'],
          options: {
            // options here to override JSHint defaults
            globals: {
              jQuery: true,
              angular: true,
              console: true,
              module: true,
              document: true
            }
          }
        },
        watch: {
          files: ['<%= jshint.files %>'],
          tasks: ['jshint']
        }
    });

    // Load required modules
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-watch');

    // Task definitions
    grunt.registerTask('launch', ['watch']);
    grunt.registerTask('test', ['jshint']);
    grunt.registerTask('default', ['concat','uglify']);
};