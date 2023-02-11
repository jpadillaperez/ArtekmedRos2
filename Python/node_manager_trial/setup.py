from setuptools import setup

package_name = 'node_manager_trial'

setup(
    name=package_name,
    version='0.0.0',
    packages=[package_name],
    data_files=[
        ('share/ament_index/resource_index/packages',
            ['resource/' + package_name]),
        ('share/' + package_name, ['package.xml']),
    ],
    install_requires=['setuptools'],
    zip_safe=True,
    maintainer='Jorge',
    maintainer_email='padillaperezjorge@gmail.com',
    description='Node receiving and sending updates of objects between unity instances',
    license='Apache License 2.0',
    tests_require=['pytest'],
    entry_points={
        'console_scripts': [
            'node_manager_trial = node_manager_trial.node_manager_trial:main'
        ],
    },
)
