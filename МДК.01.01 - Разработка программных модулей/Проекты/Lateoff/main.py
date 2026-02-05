from PyQt6 import QtCore, QtGui, QtWidgets
import subprocess
import sys
import os
import time

class Ui_MainWindow(object):
    def setupUi(self, MainWindow):
        MainWindow.setObjectName("MainWindow")
        MainWindow.resize(280, 350)
        self.centralwidget = QtWidgets.QWidget(parent=MainWindow)
        self.centralwidget.setObjectName("centralwidget")
        self.verticalLayoutWidget = QtWidgets.QWidget(parent=self.centralwidget)
        self.verticalLayoutWidget.setGeometry(QtCore.QRect(10, 10, 261, 291))
        self.verticalLayoutWidget.setObjectName("verticalLayoutWidget")
        self.verticalLayout = QtWidgets.QVBoxLayout(self.verticalLayoutWidget)
        self.verticalLayout.setContentsMargins(0, 0, 0, 0)
        self.verticalLayout.setObjectName("verticalLayout")

        self.lcdNumber = QtWidgets.QLCDNumber(parent=self.verticalLayoutWidget)
        self.lcdNumber.setDigitCount(8)
        self.lcdNumber.setSegmentStyle(QtWidgets.QLCDNumber.SegmentStyle.Flat)
        self.lcdNumber.setObjectName("lcdNumber")
        self.verticalLayout.addWidget(self.lcdNumber)

        self.label = QtWidgets.QLabel(parent=self.verticalLayoutWidget)
        self.label.setObjectName("label")
        self.verticalLayout.addWidget(self.label)

        self.spinBox = QtWidgets.QSpinBox(parent=self.verticalLayoutWidget)
        self.spinBox.setMinimum(1)
        self.spinBox.setMaximum(3600)
        self.spinBox.setValue(60)
        self.spinBox.setSuffix(" сек")
        self.spinBox.setObjectName("spinBox")
        self.verticalLayout.addWidget(self.spinBox)

        self.pushButton = QtWidgets.QPushButton(parent=self.verticalLayoutWidget)
        self.pushButton.setObjectName("pushButton")
        self.verticalLayout.addWidget(self.pushButton)

        self.pushButton_2 = QtWidgets.QPushButton(parent=self.verticalLayoutWidget)
        self.pushButton_2.setObjectName("pushButton_2")
        self.verticalLayout.addWidget(self.pushButton_2)

        self.pushButton_3 = QtWidgets.QPushButton(parent=self.verticalLayoutWidget)
        self.pushButton_3.setObjectName("pushButton_3")
        self.verticalLayout.addWidget(self.pushButton_3)

        self.statusLabel = QtWidgets.QLabel(parent=self.verticalLayoutWidget)
        self.statusLabel.setText("")
        self.statusLabel.setAlignment(QtCore.Qt.AlignmentFlag.AlignCenter)
        self.verticalLayout.addWidget(self.statusLabel)

        MainWindow.setCentralWidget(self.centralwidget)

        self.retranslateUi(MainWindow)
        QtCore.QMetaObject.connectSlotsByName(MainWindow)

    def retranslateUi(self, MainWindow):
        _translate = QtCore.QCoreApplication.translate
        MainWindow.setWindowTitle(_translate("MainWindow", "Lateoff"))
        self.label.setText(_translate("MainWindow", "Введите время (секунды):"))
        self.pushButton.setText(_translate("MainWindow", "Перезагрузка"))
        self.pushButton_2.setText(_translate("MainWindow", "Завершение работы"))
        self.pushButton_3.setText(_translate("MainWindow", "Отмена"))


class MainWindow(QtWidgets.QMainWindow):
    def __init__(self):
        super().__init__()
        self.ui = Ui_MainWindow()
        self.ui.setupUi(self)


        self.timer = QtCore.QTimer()


        self.shutdown_timer = QtCore.QTimer()
        self.shutdown_timer.timeout.connect(self.execute_shutdown)

        self.remaining_time = 0
        self.total_time = 0
        self.shutdown_type = None
        self.is_active = False

        self.ui.pushButton.clicked.connect(self.on_reload_clicked)
        self.ui.pushButton_2.clicked.connect(self.on_shutdown_clicked)
        self.ui.pushButton_3.clicked.connect(self.on_cancel_clicked)

        self.update_display()

    def update_display(self):
        minutes = self.remaining_time // 60
        seconds = self.remaining_time % 60
        self.ui.lcdNumber.display(f"{minutes:02d}:{seconds:02d}")

    def start_timer(self, seconds, shutdown_type):
        if self.is_active:
            self.cancel_timer()

        self.total_time = seconds
        self.remaining_time = seconds
        self.shutdown_type = shutdown_type
        self.is_active = True

        self.timer.start(1000)
        self.shutdown_timer.start(seconds * 1000)

        self.update_display()
        self.ui.spinBox.setEnabled(False)

        if shutdown_type == 'reboot':
            self.ui.statusLabel.setText(f"Ох... мне бы столько решимости")
            self.ui.pushButton.setEnabled(False)
        else:
            self.ui.statusLabel.setText(f"Вы решительный человек...")
            self.ui.pushButton_2.setEnabled(False)

    def cancel_timer(self):
        if self.is_active:
            self.timer.stop()
            self.shutdown_timer.stop()
            self.is_active = False

            self.ui.spinBox.setEnabled(True)
            self.ui.pushButton.setEnabled(True)
            self.ui.pushButton_2.setEnabled(True)
            self.ui.statusLabel.setText("Отменено")

            self.cancel_system_shutdown()

    def execute_shutdown(self):
        self.shutdown_timer.stop()
        self.is_active = False

        if self.shutdown_type == 'reboot':
            self.reboot_system()
        else:
            self.shutdown_system()

    def reboot_system(self):
        self.ui.statusLabel.setText("Перезагрузка...")
        QtWidgets.QApplication.processEvents()

        subprocess.run(["shutdown", "/r", "/t", "0"])
    def shutdown_system(self):
        self.ui.statusLabel.setText("Выключение...")
        QtWidgets.QApplication.processEvents()

        subprocess.run(["shutdown", "/s", "/t", "0"])

    def cancel_system_shutdown(self):
        subprocess.run(["shutdown", "/a"])


    def on_reload_clicked(self):
        seconds = self.ui.spinBox.value()
        self.start_timer(seconds, 'reboot')

    def on_shutdown_clicked(self):
        seconds = self.ui.spinBox.value()
        self.start_timer(seconds, 'shutdown')

    def on_cancel_clicked(self):
        self.cancel_timer()

    def closeEvent(self, event):
        if self.is_active:
            reply = QtWidgets.QMessageBox.question(
                self, 'Подтверждение',
                'Таймер все еще активен. Вы уверены, что хотите выйти?',
                QtWidgets.QMessageBox.StandardButton.Yes |
                QtWidgets.QMessageBox.StandardButton.No,
                QtWidgets.QMessageBox.StandardButton.No
            )

            if reply == QtWidgets.QMessageBox.StandardButton.Yes:
                self.cancel_timer()
                event.accept()
            else:
                event.ignore()
        else:
            event.accept()


if __name__ == "__main__":
    app = QtWidgets.QApplication(sys.argv)
    window = MainWindow()
    window.show()
    sys.exit(app.exec())