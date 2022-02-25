import json


# load config.json file
f = open('config.json',)
CONFIG = json.load(f)
f.close()
print(CONFIG)

healDev = CONFIG['heal_dev']

opendataCmsDev = CONFIG['opendata_cms_dev']