<?php session_start();Include_once ('mybase.class.php');?>
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
<script type="text/javascript" src="js/main_regis.js"></script>
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
    <li><a href="#tabs-1">Описание</a></li>
	<li><a href="#tabs-0">Sreenshots</a></li>
	<li><a href="#tabs-2">Настройки</a></li>
	<li><a href="#tabs-3">Что дальше</a></li>
    <li><a href="#tabs-4">API</a></li>
    <li><a href="#tabs-5">Регистрация email</a></li>
  </ul><div id="tabs-1">
<p align="left"><b>CryptoMail</b> - программа для шифрования электронной почты с использованием RSA.<br>
Как это работает:<br>
- скачиваете программу и при необходимости и NET Framework 3.5<br>
- регистрируете свой email на сервере обмена публичными ключами<br>
- подтверждаете свой email, перейдя по ссылке в ответном письме<br>
- в программе заходите в Опции (Options) и настраиваете поля по подключению к вашему серверу электронной почты,<br>
вводите ссылку на сервер обмена ключами. Сохраняете настройки.<br>
- затем заходите в генерация пары ключей (RSA Key Pair), генерируйте ключи, сохраняете и отправляете публичный ключ на сервер обмена.<br>
Если все успешно то Вы увидете сообщение "OK", в случаее ошибки "Error".<br>
- теперь заходите в Контакты (Contacts), жмете добавить контакт и вводите email.<br>
- Для того чтобы написать письмо нажмите кнопку Отправить Cryptomail (Send Cryptomail),выберите контакт<br>
нажмите Просмотреть Ключ (Show Pubkey), если ключ есть в вашей базе, то он его отобразит, иначе предложит скачать с сервера<br>
<b>Помните без ключа Вы не сможете написать сообщение.</b><br>
- Для загрузки сообщений, выберите период (по умолчанию он равен текущему дню) и нажмите Загрузить (Load Cryptomails)<br>
В списке отобразятся email тех кто Вам написал. Чтобы прочитать письмо дважды кликните в списке по отправителю.<br>
- Чтобы отправить письмо в корзину почтого сервера, выберите письмо в списке и нажмите Удалить (Delete Cryptomail).<br>
</p>
<br>Для работы необходим <a href="http://www.microsoft.com/ru-ru/download/confirmation.aspx?id=25150">NET Framework 3.5</a>
<br>Скачать: <a href="Cryptomail(en).7z">Cryptomail(en).7z</a>
<br>Github: <a href="https://github.com/Khanzo/Cryptomail">Cryptomail</a> Licence: "As is", Absolutely Free.

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
<b>Текущий Pubkey server</b> - http://rjump.net/cmailpub<br><br>
<b>mail.ru</b><br>
imap - imap.mail.ru, port - 993<br>
smtp - smtp.mail.ru, port - 25<br>
<b>gmail.com</b><br>
imap - imap.gmail.com, port - 993<br>
smtp - smtp.gmail.com, port - 25<br>
<b>yandex.ru</b><br>
imap - imap.yandex.ru, port - 993<br>
smtp - smtp.yandex.ru, port - 25<br>
</p>
</div>
<div id="tabs-3">
<p align="left">
- мульти email аккаунт<br>
- Android приложение с полной совместимостью<br>
</p>
</div>
<div id="tabs-4">
<p align="left">
<b>Получить публичный ключ</b><br>
Обращаемся к readpubkey.php Post запросом mail=email, где email - это Base64(email) запрашиваемого контакта, в Lower Case (маленькими буквами)<br>
Ответ: если контакт есть и он подтвержден - Base64(pubkey), иначе пустая строка.<br>
pubkey в ответе урезаный, для сборки нужно добавить: "&lt;RSAKeyValue&gt;&lt;Modulus&gt;" + pubkey + "&lt;/Exponent&gt;&lt;/RSAKeyValue&gt;".<br>
<b>Отправить публичный ключ</b><br>
Обращаемся к sendpubkey.php Post запросом mail=email и pubkey=pubkey, где email - это Base64(email) запрашиваемого контакта, в Lower Case (маленькими буквами),<br>
где pubkey - это Base64(урезанного pubkey), то есть без &lt;RSAKeyValue&gt;&lt;Modulus&gt;, и &lt;/Exponent&gt;&lt;/RSAKeyValue&gt;.<br>
Ответ: OK.
</p>
</div>
<div id="tabs-5">
<!-- #form -->
<div id="form"><br>
<h3 style="color:blue;">Регистрация email на сервере публичными ключами</h3><br>
  <p align="center"><p class="validateTips"></p><br>
	<label for="rmail">E-mail&nbsp</label><br>
	<input id="rmail" title="E-mail" style="width:500px;"/><br><br>
	<label for="captchareg">Captcha&nbsp;&nbsp</label><input type="text" id="captchareg" />
	<img src="captchareg.php?sid=<?php echo rand(10000, 99999); ?>" width="120" height="20" alt="" id="capreg" /><br /><br>
	<button id="registration">Отправить</button>
</div></div>
<!-- #form -->
</div>
</div><br><br><br><br><br><br>
</div><!-- #wrapper -->
<div id="footer">
<hr>
<div class="left">
© 2013 CryptoMail
</div>

<div class="right"><br><a href="http://RJump.net">Developer Khaydarov Radik</a></div>
</div><!-- #footer -->

</body>
</html>