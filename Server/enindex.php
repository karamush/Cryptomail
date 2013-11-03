<?php Include_once ('mybase.class.php');?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-type" content="text/html; charset=utf-8" />
	<title>CryptoMail</title>
	<script type="text/javascript" src="js/jquery.min.js"></script><script src="js/jquery-ui.min.js"></script>
  	<link type="text/css" href="css/smoothness/jquery-ui.css" rel="stylesheet" />
    <!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->
<link rel="stylesheet" href="css/style.css" type="text/css" media="screen, projection" />
<script type="text/javascript" src="js/enmain_regis.js"></script>
<style>
    #form {
        width: 800px;
        min-height: 300px;
        border: 1px solid #000;
        display: -moz-inline-stack;
        display: inline-block;
        vertical-align: top;
        margin: 5px;
        zoom: 1;
        *display: inline;
        _height: 350px;
-webkit-border-radius: 10px;
-moz-border-radius: 10px;
border-radius: 10px;
    }
</style>	
</head>
<body>
<div id="wrapper">
<div class="logo"><h2>CryptoMail
<img src="img/logo.png" style="margin-left:-100px"></h2>
<div style="float:right;margin-top:-50px;">
<a href="index.php"><img src="img/ru.gif"></a><br>
<a href="enindex.php"><img src="img/en.gif"></a>
</div>
</div>
<script>
  $(function() {
    $( "#tabs" ).tabs();
  });
  </script>
<div id="content" style="width:100%; text-align:center; padding-top:20px;">
<!-- #main-->
<div id="tabs">
<ul>
<li> <a href="#tabs-1"> Description </a> </li>
<li> <a href="#tabs-0"> Sreenshots </a> </li>
<li> <a href="#tabs-2"> Settings </a> </li>
<li> <a href="#tabs-3"> What's next </a> </li>
<li> <a href="#tabs-4"> API </a> </li>
<li> <a href="#tabs-5"> Register email </a> </li>
</ul> 
<div id="tabs-1">
<p align="left"> <b> CryptoMail </b> - a program for e-mail encryption using RSA. <br>
How it works : <br>
- Download the program and, if necessary , and NET Framework 3.5 <br>
- Register your email server exchange public keys <br>
- To confirm your email, by clicking on the link in the reply letter <br>
- In the program go to Options (Options) and set up a field to connect to your email server , <br>
Enter link to the server key exchange . Save the settings . <br>
- Then go to the generation of key pairs (RSA Key Pair), generate keys , save and send the public key to the server share . <br>
If successful you will see the message "OK", in case of error "Error". <br>
- Now go to Contacts (Contacts), presses to add a contact and enter the email. <br>
- In order to write a letter , press the Send button Cryptomail (Send Cryptomail), select a contact <br>
click the View key (Show Pubkey), if the key is in your database , it will display it , or prompt you to download from the server <br>
Remember <b> without key you will not be able to write a message . </b> <br>
- To download messages, select the period ( the default is the current day ), and then click Download (Load Cryptomails) <br>
Appear in the list of those who email you wrote . To read the letter in the list , double-click on the sender . <br>
- To send a letter to the basket nearly server, select the email from the list and click Remove (Delete Cryptomail). <br>
</p>
To work needed <br> <a href="http://www.microsoft.com/ru-ru/download/confirmation.aspx?id=25150"> NET Framework 3.5 </a>
<br> Download : <a href="Cryptomail(en).7z"> Cryptomail (en) .7 z </a>
<br> Github: <a href="https://github.com/Khanzo/Cryptomail"> Cryptomail </a> Licence: "As is", Absolutely Free.

</div>
<div id="tabs-0">
<p align="left">
<img src="img/screen1.jpg">
<img src="img/screen2.jpg"><br>
<img src="img/screen3.jpg">
</p>
</div>
<div id="tabs-2">
<p align="left">
<b> Current Pubkey server </b> - http://rjump.net/cmailpub <br> <br>
<b> mail.ru </b> <br>
imap - imap.mail.ru, port - 993 <br>
smtp - smtp.mail.ru, port - 25 <br>
<b> gmail.com </b> <br>
imap - imap.gmail.com, port - 993 <br>
smtp - smtp.gmail.com, port - 25 <br>
<b> yandex.ru </b> <br>
imap - imap.yandex.ru, port - 993 <br>
smtp - smtp.yandex.ru, port - 25 <br>
</p>
</div>
<div id="tabs-3">
<p align="left">
- Multi email account <br>
- Android app with full compatibility <br>
</p>
</div>
<div id="tabs-4">
<p align="left">
Get <b> public key </b> <br>
We are writing to request readpubkey.php Post mail = email, where email - it's Base64 (email) requested contact, Lower Case ( small letters ) <br>
Answer: If there is contact , and he confirmed - Base64 (pubkey), otherwise an empty string . <br>
pubkey cutting back in response to the assembly to add : "&lt;RSAKeyValue&gt;&lt;Modulus&gt;" + pubkey + "&lt;/Exponent&gt;&lt;/RSAKeyValue&gt;". <br>
Send <b> public key </b> <br>
We are writing to request sendpubkey.php Post mail = email and pubkey = pubkey, where email - it's Base64 (email) requested contact, Lower Case ( small letters ), <br>
where pubkey - it's Base64 ( truncated pubkey), that is, without "&lt;RSAKeyValue&gt;&lt;Modulus&gt;" and "&lt;/Exponent&gt;&lt;/RSAKeyValue&gt;". <br>
Reply : OK.
</p>
</div>
<div id="tabs-5">
<! - # Form ->
<div id="form"> <br>
<h3 style="color:blue;"> Register email server public keys </h3> <br><p align="center"> <p class="validateTips"> </p> <br>
<label for="rmail"> E-mail </label> <br>
<input id="rmail" title="E-mail" style="width:500px;"/> <br> <br>
<label for="captchareg"> Captcha </label> <input type="text" id="captchareg" />
<img src = "captchareg.php?sid=<?php echo rand(10000 , 99999 );?>" width = " 120 " height = " 20 " alt = "" id = "capreg" /> <br /> <br>
<button id="registration"> Send </button>
</div></div>
<! - # Form ->
</div>
</div><br><br><br><br><br><br>
</div> <! - # wrapper ->
<div id="footer">
<hr>
<div class="left">
© 2013 CryptoMail
</div>

<div class="right"> <br> <a href="http://RJump.net"> Developer Khaydarov Radik </a> </div>
</div> <! - # footer ->

</body>
</html>