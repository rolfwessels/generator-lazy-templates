'use strict';
var util = require('util');
var path = require('path');
var yeoman = require('yeoman-generator');
var chalk = require('chalk');
var yosay = require('yosay');
var uuid = require('node-uuid');


module.exports = yeoman.generators.Base.extend({
  initializing: function () {
    var self = this;
    this.pkg = require('../../package.json');
    this.copyAndReplaceFile = function(from,to) {

      var file = this.readFileAsString(from);
      var guid = uuid.v4();
      var replacedContent = file
        .replace(/MainSolutionTemplate/g,self.appName)
        .replace(/mainsolutiontemplate/g,self.appName.toLowerCase())
        .replace(/assembly\: Guid\(\"[a-z0-9\\-]+\"\)/g, "assembly: Guid(\""+guid+"\")")
        .replace(/ProductID=\"{[a-z0-9\\-]+\}\"/g, "ProductID=\"{"+guid+"}\"")
        .replace(/AppId = \"[a-z0-9\\-]+\"/g, "AppId = \""+guid+"\"")
        .replace(/\<ProjectGuid\>\{[A-z0-9\\-]+\}\<\/ProjectGuid\>/g, "<ProjectGuid>{"+guid+"}</ProjectGuid>")
        ;



      if (replacedContent != file)
      {
        self.write(to, replacedContent);
      }

      else {
        self.copy(from,to);
      }
    };
  },

  prompting: function () {
    var done = this.async();

    // Have Yeoman greet the user.
    this.log(yosay(
      'Welcome to the beautiful' + chalk.yellow('Lazy Templates') + ' generator!'
    ));

     var prompts = [{
            type    : 'input',
            name: 'appName',
            message: 'What is your app\'s name ?',
            default : this.appname,
            store   : true

        },{
            type: 'confirm',
            name: 'addDemoSection',
            message: 'Would you like to generate a demo section ?',
            default: true,
            store   : true
        }];

        this.prompt(prompts, function (answers) {
            this.appName = answers.appName;
            this.addDemoSection = answers.addDemoSection;


            this.config.save();

            done();
        }.bind(this));

  },

  saveConfig: function() {
    this.log("Save the configuration for " + this.appName);
    this.config.set('appName', this.appName);
  },

  writing: {

    projectfiles: function () {
      var templateFolder = this.templatePath('MainSolutionTemplate');
      this.log("Copy solution from '"+templateFolder+"' to '"+this.destinationRoot()+"'");
      var data = this.expandFiles("**\\*",{ cwd:templateFolder , dot : true });

      for (var i = data.length - 1; i >= 0; i--) {
        var from = templateFolder+"\\"+data[i];
        var newFileName = data[i].replace(/MainSolutionTemplate/g, this.appName);
        var to = this.destinationRoot()+"\\"+newFileName;

        this.copyAndReplaceFile(
           from,
           to
         );
      };


    }
  },

  _install: function () {
    this.installDependencies({
      skipInstall: this.options['skip-install']
    });
  }
});
