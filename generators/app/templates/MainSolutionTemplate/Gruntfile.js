module.exports = function(grunt) {

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        concat: {
            options: { "separator": ";" },
            dist: {
                "src": [
                    "src/MainSolutionTemplate.Website/scripts/**/*.js",
                    "src/MainSolutionTemplate.Website/scripts/!*.js",
                ],
                "dest": "src/MainSolutionTemplate.Website/scripts/scripts.js"
            }
        },
        uglify: {
          options: {
            banner: '/*! <%= pkg.name %> <%= grunt.template.today("yyyy-mm-dd") %> */\n'
          },
          dist: {
            files: {
              "src/MainSolutionTemplate.Website/scripts/scripts.min.js": ['<%= concat.dist.dest %>']
            }
          }
        }
    });

    // Load required modules
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');

    // Task definitions
    grunt.registerTask('default', ['concat','uglify']);
};