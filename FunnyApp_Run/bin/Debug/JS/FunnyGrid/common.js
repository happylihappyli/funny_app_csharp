String.prototype.startsWith=function(str){     
  var reg=new RegExp("^"+str);     
  return reg.test(this);        
}  

String.prototype.endsWith=function(str){     
  var reg=new RegExp(str+"$");     
  return reg.test(this);        
}