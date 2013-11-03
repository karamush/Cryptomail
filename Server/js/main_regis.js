var Base64 = {
   _keyStr : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",
    encode : function (input) {
      var output = "";
      var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
      var i = 0
      input = Base64._utf8_encode(input);
         while (i < input.length) {
       chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);
       enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;
       if( isNaN(chr2) ) {
           enc3 = enc4 = 64;
        }else if( isNaN(chr3) ){
          enc4 = 64;
        }
       output = output +
        this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
        this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);
     }
      return output;
    },
 

    decode : function (input) {
      var output = "";
      var chr1, chr2, chr3;
      var enc1, enc2, enc3, enc4;
      var i = 0;
     input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
     while (i < input.length) {
       enc1 = this._keyStr.indexOf(input.charAt(i++));
        enc2 = this._keyStr.indexOf(input.charAt(i++));
        enc3 = this._keyStr.indexOf(input.charAt(i++));
        enc4 = this._keyStr.indexOf(input.charAt(i++));
       chr1 = (enc1 << 2) | (enc2 >> 4);
        chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
        chr3 = ((enc3 & 3) << 6) | enc4;
       output = output + String.fromCharCode(chr1);
       if( enc3 != 64 ){
          output = output + String.fromCharCode(chr2);
        }
        if( enc4 != 64 ) {
          output = output + String.fromCharCode(chr3);
        }
   }
   output = Base64._utf8_decode(output);
     return output;
   },

    _utf8_encode : function (string) {
      string = string.replace(/\r\n/g,"\n");
      var utftext = "";
      for (var n = 0; n < string.length; n++) {
        var c = string.charCodeAt(n);
       if( c < 128 ){
          utftext += String.fromCharCode(c);
        }else if( (c > 127) && (c < 2048) ){
          utftext += String.fromCharCode((c >> 6) | 192);
          utftext += String.fromCharCode((c & 63) | 128);
        }else {
          utftext += String.fromCharCode((c >> 12) | 224);
          utftext += String.fromCharCode(((c >> 6) & 63) | 128);
          utftext += String.fromCharCode((c & 63) | 128);
        }
     }
      return utftext;
 
    },
 

    _utf8_decode : function (utftext) {
      var string = "";
      var i = 0;
      var c = c1 = c2 = 0;
      while( i < utftext.length ){
        c = utftext.charCodeAt(i);
       if (c < 128) {
          string += String.fromCharCode(c);
          i++;
        }else if( (c > 191) && (c < 224) ) {
          c2 = utftext.charCodeAt(i+1);
          string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
          i += 2;
        }else {
          c2 = utftext.charCodeAt(i+1);
          c3 = utftext.charCodeAt(i+2);
          string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
          i += 3;
        }
     }
     return string;
    }
 }

//-------------------------------------------------
$(document).ready(function(){

function updateTips( t ) {
      tips
        .text( t )
        .addClass( "ui-state-highlight" );
      setTimeout(function() {
        tips.removeClass( "ui-state-highlight", 1500 );		
      }, 500 );
    }
	


function checkLength( o, n, min, max ) {
      if (o.val().length < min ) {
        o.addClass( "ui-state-error" );
        updateTips( "Длина " + n + " должна быть не менее  " +min + "." );
        return false;
      } else
if (o.val().length > max ) {
        o.addClass( "ui-state-error" );
        updateTips( "Длина " + n + " должна быть не более  " +max + "." );
        return false;
      } 
	else {
        return true;
      }
    }

function checkRegexp( o, regexp, n ) {
      if ( !( regexp.test( o.val() ) ) ) {
        o.addClass( "ui-state-error" );
        updateTips( n );
        return false;
      } else {
        return true;
      }
    }

function checkLength2( o, min, max ) {
      if (o.length < min ) {
        return false;
      } else
if (o.length > max ) {
       return false;
      } 
	else {
        return true;
      }
    }

function checkRegexp2(o,regexp) {
      if ( !( regexp.test( o ) ) ) {
        return false;
      } else {
        return true;
      }
    }


$( "#registration" ).button({
      text: true,
      icons: {
        primary: "ui-icon ui-icon-check"
      }
	}).click(function() {
		var mail=$( "#rmail" ),captha=$( "#captchareg" );
		tips = $(".validateTips");
		mail.removeClass( "ui-state-error");
		captha.removeClass( "ui-state-error");
		
		if(checkLength( mail, "Email", 6, 200)==false){enbauk=0;return;}
		if(checkLength(captha, "Captcha", 6, 6)==false){enbauk=0;return;}
		if(checkRegexp(mail, /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i,'')==false){
		updateTips('Неверный Email');enbauk=0;return;}
		$.ajax({
		url: 'actions.php',type: "POST",data:{ 'act': '5','mail':Base64.encode(mail.val()),'cap':captha.val()},         
		dataType : "text",                    
		success: function (data, textStatus) {
		if(data=='not'){
		updateTips('Неверные символы');
		$("#capreg").attr( "src" , "captchareg.php?sid="+Math.random(10000, 99999) );enbauk=0;return;
		}		
		if(data=='mail'){
		updateTips('Такой email уже зарегистрирован.');
		$("#capreg").attr( "src" , "captchareg.php?sid="+Math.random(10000, 99999) );enbauk=0;return;
		}
		$('#form').html('<h3>Регистрация прошла успешно, ожидается активация.</h3>');				
		}});
	});


//---------------------------------------------------------------------------  
}); //----end
