module.exports = function(grunt) {

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        concat: {
            options: { 'separator': ';' },
            scripts: {
                'src': [
                    'MainSolutionTemplate.Website/scripts/**/*.js',
                    '!MainSolutionTemplate.Website/scripts/dist/*.js',
                ],
                'dest': 'MainSolutionTemplate.Website/scripts/dist/scripts.js'
            }            
           
        },
        uglify: {
          options: {
            banner: "/*! <%= pkg.name %> <%= grunt.template.today('yyyy-mm-dd') %> */\n"
          },
          // scripts: {
          //   files: {
          //     'MainSolutionTemplate.Website/scripts/dist/scripts.min.js': ['<%= concat.scripts.dest %>']
          //   }
          // },
          vendor: {
            files: {
              'MainSolutionTemplate.Website/scripts/dist/vendor.min.js': [
                [
                    'MainSolutionTemplate.Website/bower_components/jquery/dist/jquery.js',
                    'MainSolutionTemplate.Website/bower_components/angular/angular.js',
                    'MainSolutionTemplate.Website/bower_components/angular-route/angular-route.js',
                    'MainSolutionTemplate.Website/bower_components/angular-animate/angular-animate.js',
                    'MainSolutionTemplate.Website/bower_components/angular-local-storage/dist/angular-local-storage.js',
                    'MainSolutionTemplate.Website/bower_components/angular-loading-bar/src/loading-bar.js',                    
                    'MainSolutionTemplate.Website/bower_components/signalr/jquery.signalr.js',
                    'MainSolutionTemplate.Website/bower_components/materialize/bin/materialize.js',
                    'MainSolutionTemplate.Website/bower_components/angular-materialize/src/angular-materialize.js'
                ]
              ]
            }
          }
        },
        cssmin: {
          options: {
            shorthandCompacting: false,
            roundingPrecision: -1
          },
          target: {
            files: {
              'MainSolutionTemplate.Website/assets/css/app.min.css': [
                'MainSolutionTemplate.Website/bower_components/materialize/bin/materialize.css',
                'MainSolutionTemplate.Website/assets/css/app.css',

                ]
            }
          }
        },
        jshint: {
          files: [
            'Gruntfile.js', 
            'MainSolutionTemplate.Website/scripts/**/*.js' , 
            '!MainSolutionTemplate.Website/scripts/dist/*.js'
            ],
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
          files: ['<%= jshint.files %>' ,'MainSolutionTemplate.Website/assets/css/app.css' ],
          tasks: ['concat','cssmin','jshint']
        }
    });

    // Load required modules
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-cssmin');

    // Task definitions
    grunt.registerTask('launch', ['watch']);
    grunt.registerTask('test', ['jshint']);
    grunt.registerTask('default', ['concat','cssmin','uglify','jshint']);
};