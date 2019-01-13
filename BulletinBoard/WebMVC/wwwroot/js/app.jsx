class MassegeInformator extends React.Component {
    constructor(props) {
        super(props);
        var masseges = JSON.parse(sessionStorage.getItem('masseges'));

        var lastPublicationTime;
        if (masseges !== null) {
            if (masseges.length !== 0) {
                lastPublicationTime = new Date(masseges[masseges.length - 1].publicationDate);
            }
            else {
                lastPublicationTime = new Date(0);
            }
        }
        else {
            masseges = [];
            lastPublicationTime = new Date(0);
        }
        this.state = {
            lastPublicationTime: lastPublicationTime,
            isShownMassegeTable: false,
            shownMassegeNumber: -1,
            masseges: masseges,
            newMasseges: []
        };
        this.showOrHideMassegeTable = this.showOrHideMassegeTable.bind(this);
        this.getNewMasseges = this.getNewMasseges.bind(this);
        this.setAllMassegesAsReadOver = this.setAllMassegesAsReadOver.bind(this);
        this.setAllMassegesAsUnread = this.setAllMassegesAsUnread.bind(this);
        this.changeMassegeStatus = this.changeMassegeStatus.bind(this);
        this.showOrHideMassege = this.showOrHideMassege.bind(this);
    }
    
    componentDidMount() {
        //setInterval(this.getNewMasseges.bind(this), 5000);
    }
               
    showOrHideMassegeTable() {
        this.setState({
            isShownMassegeTable: !this.state.isShownMassegeTable,
            newItems: []
        });        
    }

    getNewMasseges() {
        // получение значений lastActionPublicationTime, items
        var lastPublicationTime;
        var masseges = this.state.masseges;
        if (masseges !== null) {
            if (masseges.length !== 0) {
                lastPublicationTime = new Date(masseges[masseges.length - 1].publicationDate);
                lastPublicationTime.setMilliseconds(lastPublicationTime.getMilliseconds() + 1);
            }
            else {
                lastPublicationTime = this.state.lastPublicationTime;
            }
        }
        else {
            lastPublicationTime = this.state.lastPublicationTime;
        }



        // получение новых сообщений
        fetch("/get-new-comments-information/" + lastPublicationTime.toISOString(), { mode: 'no-cors' })
            .then(res => res.json())
            .then(
                (result) => {
                    this.setState({
                        masseges: this.state.masseges.concat(result).slice(-30),
                        newMasseges: result,
                        lastPublicationTime: lastPublicationTime
                    });
                },
                (error) => {
                    this.setState({
                        error
                    });
                }
        );        
    }

    setAllMassegesAsReadOver() {
        var masseges = JSON.parse(sessionStorage.getItem("masseges"));
        if (masseges !== null) {
            for (let key of Object.keys(masseges)) {
                masseges[key].isUnreadMassege = false;
            }
            sessionStorage.setItem("masseges", JSON.stringify(masseges));
            this.setState({
                masseges: masseges,
                newMasseges: []
            });
        }        
    }
    
    setAllMassegesAsUnread() {
        var masseges = JSON.parse(sessionStorage.getItem("masseges"));
        if (masseges !== null) {
            for (let key of Object.keys(masseges)) {
                masseges[key].isUnreadMassege = true;
            }
            sessionStorage.setItem("masseges", JSON.stringify(masseges));
            this.setState({
                masseges: masseges,
                newMasseges: []
            });
        }
    }

    changeMassegeStatus(key) {
        var masseges = JSON.parse(sessionStorage.getItem("masseges"));
        if (masseges !== null) {
            masseges[key].isUnreadMassege = !masseges[key].isUnreadMassege;
            sessionStorage.setItem("masseges", JSON.stringify(masseges));
            this.setState({
                masseges: masseges,
                newMasseges: []
            });
        }
    }

    showOrHideMassege(key) {
        var shownMassegeNumber;
        if (this.state.shownMassegeNumber === key) {
            shownMassegeNumber = -1;            
        }
        else {
            shownMassegeNumber = key;
        }
        this.setState({
            shownMassegeNumber: shownMassegeNumber,
            newMasseges: []
        });
    }

    createMassegesHtmlListFromObjects(masseges) {
        var massegesList = [];        
        for (let key of Object.keys(masseges)) {
            var massegeClassName = "";
            var massegeStatusClassName = "";
            var massegeStatusTitle = "";
            var actionClassName;
            if (this.state.masseges[key].isUnreadMassege === true) {
                massegeClassName = "massege-informator-table-body-massege unread-massege";
                massegeStatusClassName = "massege-informator-table-body-massege-status-unread";
                massegeStatusTitle = "Отметить, как непрочитанные";
            }
            else {
                massegeClassName = "massege-informator-table-body-massege";
                massegeStatusClassName = "massege-informator-table-body-massege-status-read-over";
                massegeStatusTitle = "Отметить, как прочитанные";
            }
            if (this.state.shownMassegeNumber === key) {
                actionClassName = "massege-informator-table-body-massege-line-action-active";
            }
            else {
                actionClassName = "massege-informator-table-body-massege-line-action";
            }
            var massege = (
                <div className={massegeClassName} massege-position={key}>
                    <div className={massegeStatusClassName} data-title={massegeStatusTitle} onClick={() => { this.changeMassegeStatus(key) }}>{}</div>
                    <div className="massege-informator-table-body-massege-user-name">
                        <a href={"/user/userinfobyid/" + this.state.masseges[key].userId}>{this.state.masseges[key].userName}</a>
                    </div>
                    <div className={actionClassName} onClick={() => { this.showOrHideMassege(key) }}>
                        {this.state.masseges[key].userAction}
                    </div>
                    <div className="massege-informator-table-body-massege-line-advert-title">
                        <a href={"/advert/" + this.state.masseges[key].advertId}>{this.state.masseges[key].advertTitle}</a>
                    </div>
                </div>
            );
            massegesList.push(massege);
        }
        return massegesList;
    }

    render() {        
        // при приеме новых сообщений
        if (this.state.newMasseges.length !== 0) {
            console.log(1);
            sessionStorage.setItem("masseges", JSON.stringify(this.state.masseges));
            var audio = new Audio('/zvuk_soobscheniya_v_kontakte.mp3');
            audio.play();
        }
        var masseges = JSON.parse(sessionStorage.getItem("masseges"));

        var massegesList = [];
        var unreadMassegesNumber = 0;        
        // вычисление числа непрочитанных сообщений и формирование списка сообщений
        if (masseges !== null) {
            massegesList = this.createMassegesHtmlListFromObjects(masseges);
            for (let key of Object.keys(masseges)) {
                if (masseges[key].isUnreadMassege === true) {
                    unreadMassegesNumber++;
                }
            }
        }
        // отображение
        if (this.state.isShownMassegeTable) {
            var bottom;            
            if (this.state.shownMassegeNumber === -1) {
                bottom = (
                    <div className="control-bar">
                        <div className="control-bar-refresh" data-title="Обновить" onClick={this.getNewMasseges}>{}</div>
                        <div className="control-bar-set-all-as-read-over" data-title="Отметить все, как прочитанные" onClick={this.setAllMassegesAsReadOver}>{}</div>
                        <div className="control-bar-set-all-as-unread" data-title="Отметить все, как непрочитанные" onClick={this.setAllMassegesAsUnread}>{}</div>
                    </div>
                );
            }
            else {
                bottom = (
                    <div className="short-massege">
                        {this.state.masseges[this.state.shownMassegeNumber].commentText}
                    </div>
                );
            }
            return (
                <div className="massege-informator-table" >
                    <div className="massege-informator-table-header">
                        <div className="massege-informator-table-header-title">Сообщения</div>
                        <div className="massege-informator-table-header-close" onClick={this.showOrHideMassegeTable}>&#10006;</div>
                    </div>                    
                    <div className="massege-informator-table-body">
                        <div className="massege-informator-table-body-titles">
                            <div className="massege-informator-table-body-titles-status">{}</div>
                            <div className="massege-informator-table-body-titles-user-name">Пользователь</div>
                            <div className="massege-informator-table-body-titles-action">Действие</div>
                            <div className="massege-informator-table-body-titles-advert-title">Объявление</div>
                        </div>
                        {massegesList}
                    </div>
                    {bottom}                        
                </div>
            );
        }
        else {
            return (
                <div className="massege-informator-icon" onClick={this.showOrHideMassegeTable} >
                    <div className="massege-informator-icon-number">{unreadMassegesNumber}</div>
                </div>
            );
        }        
    }
}

ReactDOM.render(
    <MassegeInformator />,
    document.getElementById("massege-informator-box")
);