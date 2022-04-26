# Henacat_sharp?
Henacat_sharpは前橋和弥(kmaebashi)氏が作成したWebサーバソフトウェア「Henacat」をC#で実装したものです。
変数名、クラス名、メソッド名等はなるべく本家「Henacat」と対応が取れるようにつけています。
興味がある方はぜひ前橋氏の著書[「Webサーバを作りながら学ぶ 基礎からのWebアプリケーション開発入門」](http://kmaebashi.com/webserver/index.html)を読み、
実際に動かしながら学習する事をおすすめします。

Henacat_sharpについての質問等はnendocode@gmail.comまでご連絡ください。

# 環境
WindowsServer2012R2 + Visual Studio 2015 Community環境にて動作確認しています。
Henacat_sharp.slnを開き、ソリューションのビルドを実行すると、「Henacat」フォルダにHenacat_sahrp.exeが生成されます。
この実行ファイルを実行した状態で、[http://localhost:8001/](http://localhost:8001/)にアクセスするとHenacatが動作します。
デバッグモードで動作確認したい場合はスタートアッププロジェクトを「Henacat_sharp」に設定した上で実行すればデバッグモードで実行できます。
