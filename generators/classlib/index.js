'use strict';
var yeoman = require('yeoman-generator');
var uuid = require('node-uuid');
var path = require('path');

module.exports = yeoman.generators.NamedBase.extend({
  initializing: function () {
    this.log('You called the LazyTemplates subgenerator with the argument ' + this.name + '.');
    this.projectValues = {}; 
    
    // this.argument('name', {
    //   required: true,
    //   type: String,
    //   desc: 'The class name'
    // });

    

  },
  
  helperFunctions: function() {
    var self = this;
    this.replaceString = function (value,guid) {

        value = value.replace(/MainSolutionTemplate.Core/g,self.appName+"."+self.name)
        .replace(/MainSolutionTemplate/g,self.appName)
        if (guid) {
          value = value.replace(/assembly\: Guid\(\"[a-z0-9\\-]+\"\)/g, "assembly: Guid(\""+guid+"\")")
          .replace(/ProductID=\"{[a-z0-9\\-]+\}\"/g, "ProductID=\"{"+guid+"}\"")
          .replace(/AppId = \"[a-z0-9\\-]+\"/g, "AppId = \""+guid+"\"")
          .replace(/\<ProjectGuid\>\{[A-z0-9\\-]+\}\<\/ProjectGuid\>/g, "<ProjectGuid>{"+guid+"}</ProjectGuid>")
        }
        return value;

        
    }

    this.copyAndReplaceFile = function(from,to) {
     
      var file = this.readFileAsString(from);
      var replacedContent =  this.replaceString(file)
      to = path.normalize(to);
        
      var guid = uuid.v4();
      var ext = path.extname(to);
      
      if (ext == '.csproj') {
          var fileName = path.basename(to);
          self.projectValues[fileName] = { Path : to,  Guid: guid };
      }

      if (replacedContent != file)
      { 
        self.write(to, replacedContent);
      }

      else {
        self.copy(from,to);
      }
    };
  },

  loadConfig: function() {
    this.appName = this.config.get('appName');
  },

  writing:  {
    writeProjectFiles: function()   {
      this.log('Create the library files using ' + this.name + '.' + this.appName);

      var templateFolder = this.templatePath('MainSolutionTemplate');
      this.log("Copy solution from '"+templateFolder+"' to '"+this.destinationRoot()+"'");
      var data = this.expandFiles("**\\*",{ cwd:templateFolder , dot : true });
      
      for (var i = data.length - 1; i >= 0; i--) {
        var from = templateFolder+"\\"+data[i];
        var newFileName = this.replaceString(data[i]);
        var to = this.destinationRoot()+"\\"+newFileName;

        this.copyAndReplaceFile(
           from,
           to
         );
      };
    },
    install : function() {
    
    var fromSln = this.destinationRoot()+"/src/MainSolutionTemplate.sln"
    var filePath = this.replaceString(fromSln);
    var file = this.readFileAsString(filePath);
    
    var position = file.indexOf("Global");
    for(var key in this.projectValues) {
      var replace =  this.projectValues[key];
      this.log("Adding project to solution "+path.basename(replace.Path,".csproj"));
      var insert = "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \""+path.basename(replace.Path,".csproj")+"\", \""+path.relative(this.destinationRoot()+"/src", replace.Path)+"\", \"{"+replace.Guid.toUpperCase()+"}\"\r\n"+
        "EndProject\r\n";

      file = [file.slice(0, position), insert, file.slice(position)].join('');
      
    };
    
    this.write(filePath, file);
    


    // {5FFF6B98-ACEC-4FBC-80BE-2913B33B0AEE}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
    // {5FFF6B98-ACEC-4FBC-80BE-2913B33B0AEE}.Debug|Any CPU.Build.0 = Debug|Any CPU
    // {5FFF6B98-ACEC-4FBC-80BE-2913B33B0AEE}.Release|Any CPU.ActiveCfg = Release|Any CPU
    // {5FFF6B98-ACEC-4FBC-80BE-2913B33B0AEE}.Release|Any CPU.Build.0 = Release|Any CPU
    // {7D05864B-EEE0-4636-9048-0CE5321CF3DD}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
    // {7D05864B-EEE0-4636-9048-0CE5321CF3DD}.Debug|Any CPU.Build.0 = Debug|Any CPU
    // {7D05864B-EEE0-4636-9048-0CE5321CF3DD}.Release|Any CPU.ActiveCfg = Release|Any CPU
    // {7D05864B-EEE0-4636-9048-0CE5321CF3DD}.Release|Any CPU.Build.0 = Release|Any CPU
    }
  },

  
});


