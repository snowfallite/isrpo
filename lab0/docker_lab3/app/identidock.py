from flask import Flask, Response, request
import requests
import hashlib
# импортируем модуль для работы с Redis
import redis

app = Flask(__name__)
# Redis будет находиться на хосте redis
# позже в docker-compose.yml мы подключим identidock к redis
# опция port устанавливает номер порта
# опция db определяет к БД с каким номером мы подключаемся
# (в redis определено 16 логических БД с номерами от 0 до 15)
cache = redis.StrictRedis(host='redis', port=6379, db=0)
salt = "UNIQUE_SALT"
default_name = 'Artem'


@app.route('/', methods=['GET', 'POST'])
def mainpage():
	name = default_name
	if request.method =='POST':
		name = request.form['name']
	
	salted_name = salt + name
	name_hash = hashlib.sha256(salted_name.encode()).hexdigest()
	
	header = '<html><head><title>Identidock</title></head></html><body>'
	body = '''<form method="POST">
	Hello <input type="text" name="name" value="{0}">
	<input type="submit" value="submit">
	</form>
	<p>You look like a:
	<img src="/monster/{1}"/>
	'''.format(name, name_hash)
	footer = '</body></html>'
	return  header + body + footer


@app.route('/monster/<name>')
def get_identicon(name):

	# проверяем есть ли такое имя в redis
	image = cache.get(name)
	# и только если нет в БД, мы 
	# обращаемся к сервису dnmonster 
	# и добавляем вновь сгенерированное изображение
	# в БД Redis
	if image is None:
		print ("Нет в кэше", flush=True)
		r = requests.get('http://dnmonster:8080/monster/' + name + '?size=80')
		image = r.content
		cache.set(name, image)
		
	return Response(image, mimetype='image/png')


if __name__ == '__main__':
	app.run(debug=True, host='0.0.0.0')
